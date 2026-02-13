using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
