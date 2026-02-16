using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);

        Task<Product?> GetByIdAsync(int id);

        Task<List<Product>> GetPagedAsync(int pageNumber, int pageSize);

        Task<bool> ExistsAsync(Expression<Func<Product, bool>> expression);

        Task<int> CountAsync();

        Task SaveChangesAsync();
    }
}
