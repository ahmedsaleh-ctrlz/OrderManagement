using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class 
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task<int> SaveChanges();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CountAsync();
    }
}
