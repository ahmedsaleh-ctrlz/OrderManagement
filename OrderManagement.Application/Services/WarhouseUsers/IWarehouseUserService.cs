using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services.WarhouseUsers
{
    public interface IWarehouseUserService
    {
        Task AssignUserToWarehouseAsync(int userId, int warehouseId, CancellationToken ct = default);

        Task<int?> GetWarehouseIdByUserAsync(int userId, CancellationToken ct = default);

        Task<WarehouseUser> GetByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
