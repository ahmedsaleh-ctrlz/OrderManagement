using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.OrderDTOs
{
    public class OrderStatusHistoryDto
    {
        public string Status { get; set; } = default!;
        public DateTime ChangedAt { get; set; }
    }
}
