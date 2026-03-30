using OrderManagement.Application.DTOs.OrderItemDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        
        public int UserId { get; set; }
        public int WarehouseId { get; set; }   
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}
