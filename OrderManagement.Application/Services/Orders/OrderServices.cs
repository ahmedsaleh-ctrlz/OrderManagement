
using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using OrderManagement.Application.Interfaces.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Services.Users;

namespace OrderManagement.Application.Services.Orders
{

    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUserServices _userService;
        private readonly IProductRepository _productRepo;
        private readonly IProductStockRepository _stockRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<OrderServices> _logger;

        public OrderServices(
            IOrderRepository orderRepo,
            IUserServices userServices,
            IProductRepository productRepo,
            IProductStockRepository stockRepo,
            ICurrentUserService currentUser,
            ILogger<OrderServices> logger)
        {
            _orderRepo = orderRepo;
            _userService = userServices;
            _productRepo = productRepo;
            _stockRepo = stockRepo;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<PagedResult<OrderDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching orders. UserId: {UserId}, Role: {Role}", _currentUser.UserId, _currentUser.Role);

            var query = _orderRepo.GetQueryable();

            if (_currentUser.Role == "WarehouseAdmin" || _currentUser.Role == "WarehouseEmployee")
                query = query.Where(o => o.WarehouseId == _currentUser.WarehouseId);
            else if (_currentUser.Role == "Customer")
                query = query.Where(o => o.UserId == _currentUser.UserId);

            var totalCount = await query.CountAsync(ct);

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync(ct);

            _logger.LogInformation("Orders fetched. Count: {Count}, UserId: {UserId}", orders.Count, _currentUser.UserId);

            return new PagedResult<OrderDTO>
            {
                Items = orders.Select(OrderDTO.FromModel).ToList(),
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<int> CreateAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            _logger.LogInformation("Creating order. UserId: {UserId}, WarehouseId: {WarehouseId}", dto.UserId, dto.WarehouseId);

            var user = await _userService.GetUserAsync(dto.UserId, ct);
            if (user is null)
            {
                _logger.LogWarning("Order creation failed: User not found. UserId: {UserId}", dto.UserId);
                throw new NotFoundException("User not found.");
            }

            var order = new Order
            {
                UserId = dto.UserId,
                WarehouseId = dto.WarehouseId,
                Status = OrderStatus.Pending
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId, ct);
                if (product is null)
                {
                    _logger.LogWarning("Order creation failed: Product not found. ProductId: {ProductId}", item.ProductId);
                    throw new NotFoundException($"Product {item.ProductId} not found.");
                }

                var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                    s.ProductId == item.ProductId &&
                    s.WarehouseId == dto.WarehouseId, ct);

                if (stock is null)
                {
                    _logger.LogWarning("Order creation failed: Stock not found. ProductId: {ProductId}", item.ProductId);
                    throw new NotFoundException("Stock not found for selected warehouse.");
                }

                if (stock.Quantity < item.Quantity)
                {
                    _logger.LogWarning("Order creation failed: Insufficient stock. ProductId: {ProductId}", item.ProductId);
                    throw new BadRequestException("Not enough stock available.");
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = item.Quantity
                };

                total += product.Price * item.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Pending
            });

            await _orderRepo.AddAsync(order, ct);
            await _orderRepo.SaveChangesAsync(ct);

            _logger.LogInformation("Order created successfully. OrderId: {OrderId}", order.Id);

            return order.Id;
        }

        public async Task<OrderDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching order by id: {OrderId}", id);

            var order = await _orderRepo.GetWithDetailsAsync(id, ct);

            if (order is null)
            {
                _logger.LogWarning("Order not found. OrderId: {OrderId}", id);
                throw new NotFoundException("Order not found.");
            }

            return OrderDTO.FromModel(order);
        }

        public async Task ConfirmAsync(int orderId, CancellationToken ct = default)
        {
            _logger.LogInformation("Confirming order. OrderId: {OrderId}", orderId);

            var order = await _orderRepo.GetWithDetailsAsync(orderId, ct);

            if (order is null)
            {
                _logger.LogWarning("Confirm failed: Order not found. OrderId: {OrderId}", orderId);
                throw new NotFoundException("Order not found.");
            }

            if (order.Status != OrderStatus.Pending)
            {
                _logger.LogWarning("Confirm failed: Invalid status. OrderId: {OrderId}", orderId);
                throw new BadRequestException("Only pending orders can be confirmed.");
            }

            foreach (var item in order.OrderItems)
            {
                var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                    s.ProductId == item.ProductId &&
                    s.WarehouseId == order.WarehouseId, ct);

                if (stock is null || stock.Quantity < item.Quantity)
                {
                    _logger.LogWarning("Confirm failed: Stock issue. OrderId: {OrderId}", orderId);
                    throw new BadRequestException("Not enough stock.");
                }

                stock.Quantity -= item.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            order.Status = OrderStatus.Confirmed;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Confirmed
            });

            await _orderRepo.SaveChangesAsync(ct);

            _logger.LogInformation("Order confirmed. OrderId: {OrderId}", orderId);
        }

        public async Task ShipAsync(int orderId, CancellationToken ct = default)
        {
            _logger.LogInformation("Shipping order. OrderId: {OrderId}", orderId);

            var order = await _orderRepo.GetWithDetailsAsync(orderId, ct);

            if (order is null)
            {
                _logger.LogWarning("Ship failed: Order not found. OrderId: {OrderId}", orderId);
                throw new NotFoundException("Order not found.");
            }

            if (order.Status != OrderStatus.Confirmed)
            {
                _logger.LogWarning("Ship failed: Invalid status. OrderId: {OrderId}", orderId);
                throw new BadRequestException("Only confirmed orders can be shipped.");
            }

            order.Status = OrderStatus.Shipped;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Shipped
            });

            await _orderRepo.SaveChangesAsync(ct);

            _logger.LogInformation("Order shipped. OrderId: {OrderId}", orderId);
        }

        public async Task CompleteAsync(int orderId, CancellationToken ct = default)
        {
            _logger.LogInformation("Completing order. OrderId: {OrderId}", orderId);

            var order = await _orderRepo.GetWithDetailsAsync(orderId, ct);

            if (order is null)
            {
                _logger.LogWarning("Complete failed: Order not found. OrderId: {OrderId}", orderId);
                throw new NotFoundException("Order not found.");
            }

            if (order.Status != OrderStatus.Shipped)
            {
                _logger.LogWarning("Complete failed: Invalid status. OrderId: {OrderId}", orderId);
                throw new BadRequestException("Only shipped orders can be completed.");
            }

            order.Status = OrderStatus.Completed;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Completed
            });

            await _orderRepo.SaveChangesAsync(ct);

            _logger.LogInformation("Order completed. OrderId: {OrderId}", orderId);
        }

        public async Task CancelAsync(int orderId, CancellationToken ct = default)
        {
            _logger.LogInformation("Cancelling order. OrderId: {OrderId}", orderId);

            var order = await _orderRepo.GetWithDetailsAsync(orderId, ct);

            if (order is null)
            {
                _logger.LogWarning("Cancel failed: Order not found. OrderId: {OrderId}", orderId);
                throw new NotFoundException("Order not found.");
            }

            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Completed)
            {
                _logger.LogWarning("Cancel failed: Invalid status. OrderId: {OrderId}", orderId);
                throw new BadRequestException("This order cannot be cancelled.");
            }

            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var item in order.OrderItems)
                {
                    var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                        s.ProductId == item.ProductId &&
                        s.WarehouseId == order.WarehouseId, ct);

                    if (stock is not null)
                    {
                        stock.Quantity += item.Quantity;
                        stock.LastUpdated = DateTime.UtcNow;
                    }
                }
            }

            order.Status = OrderStatus.Cancelled;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Cancelled
            });

            await _orderRepo.SaveChangesAsync(ct);

            _logger.LogInformation("Order cancelled. OrderId: {OrderId}", orderId);
        }
    }
}
