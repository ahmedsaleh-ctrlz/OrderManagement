using OrderManagement.Application.DTOs.ProductStockDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Services
{
    public class ProductStockServices :IProductStockServices
    {
        #region DI
        private readonly IBaseRepository<ProductStock> _stockRepo;
        private readonly IBaseRepository<Product> _productRepo;
        private readonly IBaseRepository<Warehouse> _warehouseRepo;

        public ProductStockServices(
        IBaseRepository<ProductStock> stockRepo,
        IBaseRepository<Product> productRepo,
        IBaseRepository<Warehouse> warehouseRepo)
        {
            _stockRepo = stockRepo;
            _productRepo = productRepo;
            _warehouseRepo = warehouseRepo;
        }
        #endregion

        public async Task AddStockAsync(AddStockDTO dto)
        {
            var stock = await _stockRepo
                .FirstOrDefaultAsync(s =>
                    s.ProductId == dto.ProductId &&
                    s.WarehouseId == dto.WarehouseId);

            if (stock is null)
            {
                var product = await _productRepo.GetByIdAsync(dto.ProductId);
                var warehouse = await _warehouseRepo.GetByIdAsync(dto.WarehouseId);

                if (product is null || warehouse is null)
                    throw new NotFoundException("Product or Warehouse not found.");

                stock = new ProductStock
                {
                    ProductId = dto.ProductId,
                    WarehouseId = dto.WarehouseId,
                    Quantity = dto.Quantity,
                    LastUpdated = DateTime.UtcNow
                };

                await _stockRepo.AddAsync(stock);
            }
            else
            {
                stock.Quantity += dto.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            await _stockRepo.SaveChangesAsync();
        }


        public async Task DeductStockAsync(DeductStockDTO dto)
        {
            var stock = await _stockRepo
                .FirstOrDefaultAsync(s =>
                    s.ProductId == dto.ProductId &&
                    s.WarehouseId == dto.WarehouseId);

            if (stock is null)
                throw new NotFoundException("Stock not found.");

            if (stock.Quantity < dto.Quantity)
                throw new BadRequestException("Not enough stock available.");

            stock.Quantity -= dto.Quantity;
            stock.LastUpdated = DateTime.UtcNow;
            await _stockRepo.SaveChangesAsync();
         
        }

        public async Task<int> GetQuantityAsync(ProductQuantityDTO DTO)
        {
            var stock = await _stockRepo
                .FirstOrDefaultAsync(s =>
                    s.ProductId == DTO.ProductId &&
                    s.WarehouseId == DTO.WarehouseId);

            return stock?.Quantity ?? 0;
        }




    }
}
