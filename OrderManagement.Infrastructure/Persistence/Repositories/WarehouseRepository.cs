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

    public async Task AddAsync(Warehouse warehouse, CancellationToken ct = default)
    {
        await _context.Warehouses.AddAsync(warehouse,ct);
    }

    public async Task<Warehouse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == id,ct);
    }

    public async Task<List<Warehouse>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        return await _context.Warehouses
            .OrderBy(w => w.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Warehouse, bool>> expression, CancellationToken ct = default)
    {
        return await _context.Warehouses.AnyAsync(expression,ct);
    }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        return await _context.Warehouses.CountAsync(ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
