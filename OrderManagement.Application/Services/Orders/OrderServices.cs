using OrderManagement.Application.DTOs.OrderItemDTOs;
using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using OrderManagement.Application.DTOs.Paging;


namespace OrderManagement.Application.Services.Orders
{


    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUserRepository _userRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductStockRepository _stockRepo;

        public OrderServices(
            IOrderRepository orderRepo,
            IUserRepository userRepo,
            IProductRepository productRepo,
            IProductStockRepository stockRepo)
        {
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _productRepo = productRepo;
            _stockRepo = stockRepo;
        }

        public async Task<PagedResult<OrderDTO>> GetPagedAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var orders = await _orderRepo.GetPagedAsync(param.PageNumber, param.PageSize);
            var totalCount = await _orderRepo.CountAsync();

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



                stock.LastUpdated = DateTime.UtcNow;

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

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            return order.Id;
        }

        public async Task<OrderDTO> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetWithItemsAsync(id);
            if (order is null)
                throw new NotFoundException("Order not found.");
            return MapToDto(order);
        }
        public async Task ConfirmAsync(int orderId)
        {
            var order = await _orderRepo.GetWithItemsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status != OrderStatus.Pending)
                throw new BadRequestException("Only pending orders can be confirmed.");

            foreach (var item in order.OrderItems)
            {
                var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                    s.ProductId == item.ProductId);

                if (stock is null || stock.Quantity < item.Quantity)
                    throw new BadRequestException("Not enough stock.");

                stock.Quantity -= item.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            order.Status = OrderStatus.Confirmed;

            await _orderRepo.SaveChangesAsync();
        }

        public async Task CancelAsync(int orderId)
        {
            var order = await _orderRepo.GetWithItemsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status == OrderStatus.Shipped)
                throw new BadRequestException("Shipped orders cannot be cancelled.");

            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var item in order.OrderItems)
                {
                    var stock = await _stockRepo.FirstOrDefaultAsync(s =>
                        s.ProductId == item.ProductId);

                    if (stock is not null)
                    {
                        stock.Quantity += item.Quantity;
                        stock.LastUpdated = DateTime.UtcNow;
                    }
                }
            }

            order.Status = OrderStatus.Cancelled;

            await _orderRepo.SaveChangesAsync();
        }

        public async Task ShipAsync(int orderId)
        {
            var order = await _orderRepo.GetWithItemsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status != OrderStatus.Confirmed)
                throw new BadRequestException("Only confirmed orders can be shipped.");

            order.Status = OrderStatus.Shipped;

            await _orderRepo.SaveChangesAsync();
        }

        public async Task CompleteAsync(int orderId)
        {
            var order = await _orderRepo.GetWithItemsAsync(orderId);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status != OrderStatus.Shipped)
                throw new BadRequestException("Only shipped orders can be completed.");

            order.Status = OrderStatus.Completed;

            await _orderRepo.SaveChangesAsync();
        }


        //Helper Methods
        public OrderDTO MapToDto(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(i => new OrderItemDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };

        }
    }

}