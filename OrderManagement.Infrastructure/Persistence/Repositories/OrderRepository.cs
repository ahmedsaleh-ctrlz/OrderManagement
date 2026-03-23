using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

      

        public async Task AddAsync(Order order , CancellationToken ct = default!)
        {
            await _context.Orders.AddAsync(order,ct);
        }

     

        public async Task<bool> ExistsAsync(Expression<Func<Order, bool>> predicate, CancellationToken ct = default!)
        {
            return await _context.Orders.AnyAsync(predicate,ct);
        }

      
        public async Task<Order?> GetWithDetailsAsync(int id, CancellationToken ct = default!)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Warehouse)
                .Include(o => o.OrderItems)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.Id == id,ct);
        }



        public IQueryable<Order> GetQueryable()
        {
            return _context.Orders
                .Include(o=>o.User)
                .Include(o => o.Warehouse)
                .Include(o => o.OrderItems)
                .Include(o => o.StatusHistory)
                .AsNoTracking()
                .AsQueryable();
        }



        public async Task<int> CountAsync(CancellationToken ct = default!)
        {
            return await _context.Orders.CountAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default!)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
