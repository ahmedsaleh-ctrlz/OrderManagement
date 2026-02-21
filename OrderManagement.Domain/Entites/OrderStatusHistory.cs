using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;

public class OrderStatusHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public OrderStatus Status { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
