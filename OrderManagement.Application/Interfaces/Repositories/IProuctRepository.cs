using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);

        Task<Product?> GetByIdAsync(int id);



        Task<bool> ExistsAsync(Expression<Func<Product, bool>> expression);

      
        

        Task SaveChangesAsync();
    }
}
