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
        public string Name { get; set; } = default!;
        public string? SKU { get; set; }
        public decimal Price { get; set; }
    }
}
