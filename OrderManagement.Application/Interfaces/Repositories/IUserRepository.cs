using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken ct = default);

        Task<User?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<User?> FirstOrDefaultAsync(
            Expression<Func<User, bool>> expression, CancellationToken ct = default);

        Task<List<User>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<User, bool>>? expression = null , CancellationToken ct = default);
        Task<bool> ExistsAsync(
            Expression<Func<User, bool>> expression, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<User, bool>>? expression = null, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }

}
