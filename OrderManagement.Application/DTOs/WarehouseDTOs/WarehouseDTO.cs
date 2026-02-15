using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.WarehouseDTOs
{
    public class WarehouseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = default!;
    }
}
