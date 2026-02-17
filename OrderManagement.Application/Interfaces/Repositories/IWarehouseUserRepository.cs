using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IWarehouseUserRepository
    {
        Task AddAsync(WarehouseUser entity);

        Task<WarehouseUser?> GetByUserIdAsync(int userId);

        Task<bool> ExistsAsync(int userId);

        Task SaveChangesAsync();
    }
}
