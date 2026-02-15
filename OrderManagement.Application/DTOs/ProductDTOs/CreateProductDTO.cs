using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string SKU { get; set; } = default!;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int WarehouseId { get; set; }

        [Range(0, int.MaxValue)]
        public int InitialQuantity { get; set; }
    }
}
