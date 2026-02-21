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

      

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

     

        public async Task<bool> ExistsAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders.AnyAsync(predicate);
        }

      
        public async Task<Order?> GetWithDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Warehouse)
                .Include(o => o.OrderItems)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.Id == id);
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



        public async Task<int> CountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
