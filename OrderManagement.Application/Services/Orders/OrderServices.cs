using OrderManagement.Application.DTOs.OrderItemDTOs;
using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using OrderManagement.Application.Interfaces.Global;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Application.Services.Orders
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUserRepository _userRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductStockRepository _stockRepo;
        private readonly ICurrentUserService _currentUser;

        public OrderServices(
            IOrderRepository orderRepo,
            IUserRepository userRepo,
            IProductRepository productRepo,
            IProductStockRepository stockRepo,
            ICurrentUserService currentUser)
        {
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _productRepo = productRepo;
            _stockRepo = stockRepo;
            _currentUser = currentUser;
        }



        public async Task<PagedResult<OrderDTO>> GetPagedAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var query = _orderRepo.GetQueryable();

           
            if (_currentUser.Role == "WarehouseAdmin" ||
                _currentUser.Role == "WarehouseEmployee")
            {
            
                query = query.Where(o => o.WarehouseId == _currentUser.WarehouseId);
            }
            else if (_currentUser.Role == "Customer")
            {
                query = query.Where(o => o.UserId == _currentUser.UserId);
            }
         
            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<OrderDTO>
            {
                Items = orders.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }




        public async Task<int> CreateAsync(CreateOrderDTO dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user is null)
                throw new NotFoundException("User not found.");

            var order = new Order
            {
                UserId = dto.UserId,
                WarehouseId = dto.WarehouseId,
                Status = OrderStatus.Pending
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product is null)
                    throw new NotFoundException($"Product {item.ProductId} not found.");

                var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                    s.ProductId == item.ProductId &&
                    s.WarehouseId == dto.WarehouseId);

                if (stock is null)
                    throw new NotFoundException("Stock not found for selected warehouse.");

                if (stock.Quantity < item.Quantity)
                    throw new BadRequestException("Not enough stock available.");

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

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            return order.Id;
        }

     
        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetWithDetailsAsync(id);

            if (order is null)
                throw new NotFoundException("Order not found.");

            return MapToDto(order);
        }

    

        public async Task ConfirmAsync(int orderId)
        {
            var order = await _orderRepo.GetWithDetailsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status != OrderStatus.Pending)
                throw new BadRequestException("Only pending orders can be confirmed.");

            foreach (var item in order.OrderItems)
            {
                var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                    s.ProductId == item.ProductId &&
                    s.WarehouseId == order.WarehouseId);

                if (stock is null || stock.Quantity < item.Quantity)
                    throw new BadRequestException("Not enough stock.");

                stock.Quantity -= item.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            order.Status = OrderStatus.Confirmed;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Confirmed
            });

            await _orderRepo.SaveChangesAsync();
        }

 

        public async Task ShipAsync(int orderId)
        {
            var order = await _orderRepo.GetWithDetailsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status != OrderStatus.Confirmed)
                throw new BadRequestException("Only confirmed orders can be shipped.");

            order.Status = OrderStatus.Shipped;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Shipped
            });

            await _orderRepo.SaveChangesAsync();
        }

 

        public async Task CompleteAsync(int orderId)
        {
            var order = await _orderRepo.GetWithDetailsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status != OrderStatus.Shipped)
                throw new BadRequestException("Only shipped orders can be completed.");

            order.Status = OrderStatus.Completed;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                Status = OrderStatus.Completed
            });

            await _orderRepo.SaveChangesAsync();
        }



        public async Task CancelAsync(int orderId)
        {
            var order = await _orderRepo.GetWithDetailsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status == OrderStatus.Shipped ||
                order.Status == OrderStatus.Completed)
                throw new BadRequestException("This order cannot be cancelled.");

            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var item in order.OrderItems)
                {
                    var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                        s.ProductId == item.ProductId &&
                        s.WarehouseId == order.WarehouseId);

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

            await _orderRepo.SaveChangesAsync();
        }

     

        private OrderDTO MapToDto(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                UserName = order.User?.FullName ?? "",
                UserEmail = order.User?.Email ?? "",
                WarehouseName = order.Warehouse?.Name ?? "",
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,

                Items = order.OrderItems.Select(i => new OrderItemDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList(),

                 History = order.StatusHistory
                    .OrderBy(h => h.ChangedAt)
                    .Select(h => new OrderStatusHistoryDto
                    {
                        Status = h.Status.ToString(),
                        ChangedAt = h.ChangedAt
                    }).ToList()
            };
        }
    }
}
