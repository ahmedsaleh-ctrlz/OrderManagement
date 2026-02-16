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

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        
    }

    public async Task<User?> FirstOrDefaultAsync(
        Expression<Func<User, bool>> expression)
    {
        return await _context.Users
            .FirstOrDefaultAsync(expression);
    }

    public async Task<List<User>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<User, bool>>? expression = null)
    {
        IQueryable<User> query = _context.Users;

        if (expression != null)
            query = query.Where(expression);

        return await query
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<User, bool>> expression)
    {
        return await _context.Users.AnyAsync(expression);
    }

    public async Task<int> CountAsync(
        Expression<Func<User, bool>>? expression = null)
    {
        if (expression == null)
            return await _context.Users.CountAsync();

        return await _context.Users.CountAsync(expression);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
