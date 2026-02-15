using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.ProductStockDTOs
{
    public class DeductStockDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int WarehouseId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
