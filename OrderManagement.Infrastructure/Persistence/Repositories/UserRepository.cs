using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Infrastructure.Persistence;
using System.Linq.Expressions;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _context.Users.AddAsync(user,ct);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id,ct);
        
    }

    public async Task<User?> FirstOrDefaultAsync(
        Expression<Func<User, bool>> expression, CancellationToken ct = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(expression,ct);
    }

    public async Task<List<User>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<User, bool>>? expression = null, CancellationToken ct = default)
    {
        IQueryable<User> query = _context.Users;

        if (expression != null)
            query = query.Where(expression);

        return await query
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<User, bool>> expression, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(expression,ct);
    }

    public async Task<int> CountAsync(
        Expression<Func<User, bool>>? expression = null, CancellationToken ct = default)
    {
        if (expression == null)
            return await _context.Users.CountAsync(ct);

        return await _context.Users.CountAsync(expression,ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
