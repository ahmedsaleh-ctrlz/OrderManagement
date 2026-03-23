using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;


namespace OrderManagement.Infrastructure.Persistence.Repositories
{
    public class WarehouseUserRepository : IWarehouseUserRepository
    {
        private readonly AppDbContext _context;

        public WarehouseUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WarehouseUser entity, CancellationToken ct = default)
        {
            await _context.WarehouseUsers.AddAsync(entity,ct);
        }

        public async Task<WarehouseUser?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        {
            return await _context.WarehouseUsers
                .FirstOrDefaultAsync(x => x.UserId == userId,ct);
        }

        public async Task<bool> ExistsAsync(int userId, CancellationToken ct = default)
        {
            return await _context.WarehouseUsers
                .AnyAsync(x => x.UserId == userId,ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public IQueryable<WarehouseUser> GetQueryable() 
        {
            return _context.WarehouseUsers.
                Include(x => x.User)
                .Include(x => x.Warehouse)
                .AsNoTracking()
                .AsQueryable();
        }
    }
}
