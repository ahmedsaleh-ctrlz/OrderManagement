using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IProductStockRepository
    {

        Task<ProductStock?> GetAsync(
         Expression<Func<ProductStock, bool>> expression, CancellationToken ct = default);

        Task AddAsync(ProductStock stock, CancellationToken ct = default);
        Task<ProductStock> FirstOrDefaultAsync(Expression<Func<ProductStock, bool>> expression, CancellationToken ct = default);

        IQueryable<ProductStock> GetQueryable();

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
