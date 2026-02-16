using OrderManagement.Application.DTOs.OrderItemDTOs;
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

        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
