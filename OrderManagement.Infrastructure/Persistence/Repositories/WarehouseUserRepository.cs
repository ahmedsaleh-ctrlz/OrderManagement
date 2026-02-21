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

        public async Task AddAsync(WarehouseUser entity)
        {
            await _context.WarehouseUsers.AddAsync(entity);
        }

        public async Task<WarehouseUser?> GetByUserIdAsync(int userId)
        {
            return await _context.WarehouseUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _context.WarehouseUsers
                .AnyAsync(x => x.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
