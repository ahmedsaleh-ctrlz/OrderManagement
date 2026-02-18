using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IWarehouseRepository
    {
        Task AddAsync(Warehouse warehouse);

        Task<Warehouse?> GetByIdAsync(int id);

        Task<List<Warehouse>> GetPagedAsync(int pageNumber, int pageSize);

        Task<bool> ExistsAsync(Expression<Func<Warehouse, bool>> expression);

        Task<int> CountAsync();

        Task SaveChangesAsync();
    }
}
