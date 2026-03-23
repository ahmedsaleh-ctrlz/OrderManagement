using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
       
        private UserDTO() { }

        public static UserDTO FromModel(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
