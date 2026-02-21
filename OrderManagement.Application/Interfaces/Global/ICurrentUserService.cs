using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Global
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Role { get; }
        int? WarehouseId { get; }
    }
}
