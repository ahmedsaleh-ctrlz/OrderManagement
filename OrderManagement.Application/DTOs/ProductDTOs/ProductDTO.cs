using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public decimal Price { get; set; }

        [JsonConstructor]
        private ProductDTO() { }

        public static ProductDTO FromModel(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price
            };
        }

        public static Expression<Func<ProductStock, ProductDTO>> Selector =
            ps => new ProductDTO
        {
            Id = ps.Product.Id,
            Name = ps.Product.Name,
            Price = ps.Product.Price,
            SKU = ps.Product.SKU
        };

    }
}
