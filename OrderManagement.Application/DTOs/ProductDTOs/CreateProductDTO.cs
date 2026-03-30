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
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public decimal Price { get; set; }  
        public int InitialQuantity { get; set; }
    }
}
