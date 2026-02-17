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
        [Required]
        public int UserId { get; set; }

        public int WarehouseId { get; set; }   
        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}
