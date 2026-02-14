using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entites
{
    public class Warehouse
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Location { get; set; } = default!;

        public bool IsDeleted { get; set; }
    }
}
