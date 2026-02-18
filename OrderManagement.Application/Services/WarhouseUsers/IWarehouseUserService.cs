using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services.WarhouseUsers
{
    public interface IWarehouseUserService
    {
        Task AssignUserToWarehouseAsync(int userId, int warehouseId);

        Task<int?> GetWarehouseIdByUserAsync(int userId);
    }
}
