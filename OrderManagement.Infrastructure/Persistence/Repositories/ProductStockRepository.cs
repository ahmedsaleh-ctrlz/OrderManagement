using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using System.Linq.Expressions;

namespace OrderManagement.Infrastructure.Persistence.Repositories
{
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly AppDbContext _context;

        public ProductStockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductStock?> GetAsync(
            Expression<Func<ProductStock, bool>> expression, CancellationToken ct = default)
        {
            return await _context.ProductStocks
                .FirstOrDefaultAsync(expression,ct);
        }

        public async Task AddAsync(ProductStock stock,CancellationToken ct = default)
        {
            await _context.ProductStocks.AddAsync(stock,ct);
        }
        
        public async Task<ProductStock?> FirstOrDefaultAsync(Expression<Func<ProductStock, bool>> expression, CancellationToken ct = default) 
        {
            return await _context.ProductStocks.FirstOrDefaultAsync(expression,ct);
        }
        
        public IQueryable<ProductStock> GetQueryable() 
        {
            return _context.ProductStocks.AsNoTracking().AsQueryable();
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}