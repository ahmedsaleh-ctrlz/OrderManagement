using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task<User?> GetByIdAsync(int id);

        Task<User?> FirstOrDefaultAsync(
            Expression<Func<User, bool>> expression);

        Task<List<User>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<User, bool>>? expression = null);
        Task<bool> ExistsAsync(
            Expression<Func<User, bool>> expression);
        Task<int> CountAsync(Expression<Func<User, bool>>? expression = null);
        Task SaveChangesAsync();
    }

}
