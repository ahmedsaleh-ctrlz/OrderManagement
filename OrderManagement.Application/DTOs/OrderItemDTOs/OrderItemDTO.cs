
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.DTOs.OrderItemDTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        private OrderItemDTO() { }

        public static OrderItemDTO FromModel(OrderItem item) 
        {
            return new OrderItemDTO
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            };
        }

        public static IEnumerable<OrderItemDTO> FromModels(IEnumerable<OrderItem> items)
        {
            return items.Select(FromModel);
        }
    }
}
