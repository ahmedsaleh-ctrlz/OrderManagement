using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.WarehouseDTOs
{
    public class UpdateWarehouseDTO
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = default!;
    }
}
