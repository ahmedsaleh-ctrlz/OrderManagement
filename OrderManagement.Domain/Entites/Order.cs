using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entites
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderStatusHistory> StatusHistory { get; set; }
         = new List<OrderStatusHistory>();

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public byte[] RowVersion { get; set; } = default!;  
    }
}
