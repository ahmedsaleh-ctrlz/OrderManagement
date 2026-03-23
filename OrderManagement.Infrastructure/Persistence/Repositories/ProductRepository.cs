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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken ct = default)
        {
            await _context.Products.AddAsync(product,ct);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id,ct);
        }


        public async Task<bool> ExistsAsync(Expression<Func<Product, bool>> expression, CancellationToken ct = default)
        {
            return await _context.Products.AnyAsync(expression,ct);
        }


        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
        
    }
}
