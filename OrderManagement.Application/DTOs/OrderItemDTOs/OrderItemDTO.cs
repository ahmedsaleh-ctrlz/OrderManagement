
namespace OrderManagement.Application.DTOs.OrderItemDTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = default!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
