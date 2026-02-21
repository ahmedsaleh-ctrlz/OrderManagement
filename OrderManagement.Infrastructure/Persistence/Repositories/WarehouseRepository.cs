using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Infrastructure.Persistence;
using System.Linq.Expressions;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Warehouse warehouse)
    {
        await _context.Warehouses.AddAsync(warehouse);
    }

    public async Task<Warehouse?> GetByIdAsync(int id)
    {
        return await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Warehouse>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Warehouses
            .OrderBy(w => w.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<Warehouse, bool>> expression)
    {
        return await _context.Warehouses.AnyAsync(expression);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Warehouses.CountAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
