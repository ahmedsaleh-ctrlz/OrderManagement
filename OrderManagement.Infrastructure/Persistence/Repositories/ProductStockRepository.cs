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
            Expression<Func<ProductStock, bool>> expression)
        {
            return await _context.ProductStocks
                .FirstOrDefaultAsync(expression);
        }

        public async Task AddAsync(ProductStock stock)
        {
            await _context.ProductStocks.AddAsync(stock);
        }
        
        public async Task<ProductStock?> FirstOrDefaultAsync(Expression<Func<ProductStock, bool>> expression) 
        {
            return await _context.ProductStocks.FirstOrDefaultAsync(expression);
        }
        public async Task<List<ProductStock>> GetByWarehouseIdAsync(int warehouseId)
        {
            return await _context.ProductStocks
                .Where(ps => ps.WarehouseId == warehouseId)
                .Include(ps => ps.Product)
                .ToListAsync();
        }

            public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}