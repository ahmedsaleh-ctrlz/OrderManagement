using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IProductStockRepository
    {

        Task<ProductStock?> GetAsync(
         Expression<Func<ProductStock, bool>> expression);

        Task AddAsync(ProductStock stock);
        Task<ProductStock> FirstOrDefaultAsync(Expression<Func<ProductStock, bool>> expression);

        IQueryable<ProductStock> GetQueryable();

        Task SaveChangesAsync();
    }
}
