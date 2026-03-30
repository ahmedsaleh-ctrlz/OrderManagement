using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.WarehouseDTOs
{
    public class CreateWarehouseDTO
    {
        public string Name { get; set; } = null!;
        public string Location { get; set; } = default!;    
    }
}
