using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

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
