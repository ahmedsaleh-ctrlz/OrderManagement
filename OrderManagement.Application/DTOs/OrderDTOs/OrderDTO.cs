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

        public string UserName { get; set; } = default!;
        public string UserEmail { get; set; }
        public string WarehouseName { get; set; } = default!;

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();

        public List<OrderStatusHistoryDto> History { get; set; } = new();

    }
}
