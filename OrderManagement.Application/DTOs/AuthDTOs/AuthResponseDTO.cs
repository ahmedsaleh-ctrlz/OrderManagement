using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.AuthDTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = default!;
        public DateTime Expiration { get; set; }
    }
}
