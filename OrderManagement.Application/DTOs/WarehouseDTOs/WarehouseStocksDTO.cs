using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.WarehouseDTOs
{
    public class WarehouseStockDTO
    {
       

        public string ProductName { get; set; } = default!;

        public string SKU { get; set; } = default!;
        public decimal UnitPrice { get; set; } = default!;

        public int Quantity { get; set; }
        
    }

}
