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
        [Required(ErrorMessage = "Warehouse name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Location is required")]
        [MaxLength(200)]
        public string Location { get; set; } = default!;    
    }
}
