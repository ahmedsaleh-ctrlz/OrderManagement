using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = default!;
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = default!;
    }
}
