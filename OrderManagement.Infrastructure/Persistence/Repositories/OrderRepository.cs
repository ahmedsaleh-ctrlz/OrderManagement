using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Order?> GetWithItemsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Orders.CountAsync();
        }
        public async Task<List<Order>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
