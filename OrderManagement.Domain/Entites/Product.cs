using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entites
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string SKU { get; set; } = default!;

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

       
    }
}
