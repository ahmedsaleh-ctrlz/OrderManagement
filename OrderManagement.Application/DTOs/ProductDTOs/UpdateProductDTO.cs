using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
