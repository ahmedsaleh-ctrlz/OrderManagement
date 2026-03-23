using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IWarehouseRepository
    {
        Task AddAsync(Warehouse warehouse, CancellationToken ct = default);

        Task<Warehouse?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<List<Warehouse>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);

        Task<bool> ExistsAsync(Expression<Func<Warehouse, bool>> expression, CancellationToken ct = default);

        Task<int> CountAsync(CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
