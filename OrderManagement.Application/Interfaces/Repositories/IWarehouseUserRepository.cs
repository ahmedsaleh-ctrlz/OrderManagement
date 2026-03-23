using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IWarehouseUserRepository
    {
        Task AddAsync(WarehouseUser entity, CancellationToken ct = default);

        Task<WarehouseUser?> GetByUserIdAsync(int userId, CancellationToken ct = default);

        Task<bool> ExistsAsync(int userId, CancellationToken ct = default);

        IQueryable<WarehouseUser> GetQueryable();

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
