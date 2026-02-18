using OrderManagement.Domain.Entites;

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
