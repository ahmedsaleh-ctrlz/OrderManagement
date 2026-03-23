using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product,CancellationToken ct = default);

        Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);



        Task<bool> ExistsAsync(Expression<Func<Product, bool>> expression, CancellationToken ct = default);

      
        

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
