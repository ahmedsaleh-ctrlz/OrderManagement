using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Repositories
{
    public interface IProductStockRepository
    {

        Task<ProductStock?> GetAsync(
         Expression<Func<ProductStock, bool>> expression);

        Task AddAsync(ProductStock stock);
        Task<ProductStock> FirstOrDefaultAsync(Expression<Func<ProductStock, bool>> expression);

        Task<List<ProductStock>> GetByWarehouseIdAsync(int warehouseId);

        Task SaveChangesAsync();
    }
}
