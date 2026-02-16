using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entites
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!; // For display purposes, we store the product name at the time of order

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        

    }

}
