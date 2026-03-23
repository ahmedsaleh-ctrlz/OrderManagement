using OrderManagement.Application.DTOs.OrderItemDTOs;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; } = default!;
        public string UserEmail { get; set; }
        public string WarehouseName { get; set; } = default!;

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();

        public List<OrderStatusHistoryDto> History { get; set; } = new();

        private OrderDTO() { }

        public static OrderDTO FromModel(Order order) 
        {
            return new OrderDTO
            {
                Id = order.Id,
                UserName = order.User.FullName,
                UserEmail = order.User.Email,
                WarehouseName = order.Warehouse.Name,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Items = OrderItemDTO.FromModels(order.OrderItems).ToList(),
                History = order.StatusHistory.Select(h => new OrderStatusHistoryDto
                {
                    Status = h.Status.ToString(),
                    ChangedAt = h.ChangedAt
                }).ToList()
            };
        }

    }
}
