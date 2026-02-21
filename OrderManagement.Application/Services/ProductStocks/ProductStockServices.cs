
using OrderManagement.Application.DTOs.ProductStockDTOs;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Services.ProductStocks
{
    public class ProductStockServices : IProductStockServices
    {
        private readonly IProductStockRepository _repo;

        public ProductStockServices(IProductStockRepository repo)
        {
            _repo = repo;
        }

        public async Task AddStockAsync(AddStockDTO dto)
        {
            var stock = await _repo.GetAsync(
                s => s.ProductId == dto.ProductId &&
                     s.WarehouseId == dto.WarehouseId);

            if (stock is null)
            {
                stock = new ProductStock
                {
                    ProductId = dto.ProductId,
                    WarehouseId = dto.WarehouseId,
                    Quantity = dto.Quantity
                };

                await _repo.AddAsync(stock);
            }
            else
            {
                stock.Quantity += dto.Quantity;
            }

            await _repo.SaveChangesAsync();
        }

        public async Task DeductStockAsync(DeductStockDTO dto)
        {
            var stock = await _repo.GetAsync(
                s => s.ProductId == dto.ProductId &&
                     s.WarehouseId == dto.WarehouseId);

            if (stock is null)
                throw new NotFoundException("Stock not found");

            if (stock.Quantity < dto.Quantity)
                throw new BadRequestException("Insufficient stock");

            stock.Quantity -= dto.Quantity;

            await _repo.SaveChangesAsync();
        }

        public async Task<int> GetQuantityAsync(ProductQuantityDTO dto)
        {
            var stock = await _repo.GetAsync(
                s => s.ProductId == dto.ProductId &&
                     s.WarehouseId == dto.WarehouseId);

            if (stock is null)
                return 0;

            return stock.Quantity;
        }

  
    }
}
