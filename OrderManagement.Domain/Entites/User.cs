using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entites
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        
    }

}
