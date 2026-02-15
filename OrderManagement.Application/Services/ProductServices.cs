using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IBaseRepository<Product> _productRepo;
        private readonly IBaseRepository<ProductStock> _stockRepo;
        private readonly IBaseRepository<Warehouse> _warehouseRepo;

        public ProductServices(IBaseRepository<Product> productRepo,
            IBaseRepository<ProductStock> stockRepo,IBaseRepository<Warehouse> warehouseRepo)
        {
            _productRepo = productRepo;
            _stockRepo = stockRepo;
            _warehouseRepo = warehouseRepo;
            
        }


        public async Task<PagedResult<ProductDTO>> GetPagedAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var products = await _productRepo.GetPagedAsync(param.PageNumber, param.PageSize);
            var totalCount = await _productRepo.CountAsync();

            var mapped = products.Select(MapToDTO).ToList();

            return new PagedResult<ProductDTO>
            {
                Items = mapped,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<int> CreateAsync(CreateProductDTO dto)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(dto.WarehouseId);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found.");

            var normalizedSku = dto.SKU.Trim().ToUpper();

            var exists = await _productRepo.ExistsAsync(p => p.SKU == normalizedSku);
            if (exists)
                throw new BadRequestException("SKU already exists.");

            var product = new Product
            {
                Name = dto.Name,
                SKU = normalizedSku,
                Price = dto.Price
            };

            await _productRepo.AddAsync(product);

            var stock = new ProductStock
            {
                Product = product, 
                WarehouseId = dto.WarehouseId,
                Quantity = dto.InitialQuantity,
                LastUpdated = DateTime.UtcNow
                
            };

            await _stockRepo.AddAsync(stock);

           
            await _productRepo.SaveChangesAsync();

            return product.Id;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
                throw new NotFoundException("Product not found");

            return MapToDTO(product);
        }

        public async Task UpdateAsync(int id, UpdateProductDTO DTO)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
                throw new NotFoundException("Product not found");

            product.Name = DTO.Name;
            product.Price = DTO.Price;

            await _productRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
                throw new NotFoundException("Product not found");

            product.IsDeleted = true;

            await _productRepo.SaveChangesAsync();
        }

        private static ProductDTO MapToDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price
            };
        }
    }
}
