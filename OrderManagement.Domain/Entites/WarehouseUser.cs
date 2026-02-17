using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entites
{
    public class WarehouseUser
    {
        public int Id  { get; set; }
        public User User { get; set; } = default!;
        public int UserId { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;    
    }
}
