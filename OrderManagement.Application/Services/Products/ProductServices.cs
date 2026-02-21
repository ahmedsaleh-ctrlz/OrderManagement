using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Global;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;


namespace OrderManagement.Application.Services.Products
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepo;
        private readonly IWarehouseRepository _warehouseRepo;
        private readonly IProductStockRepository _stockRepo;
        private readonly ICurrentUserService _currentUser;
        


        public ProductServices(
            IProductRepository productRepo,
            IWarehouseRepository warehouseRepo,
            IProductStockRepository stockRepo,
            ICurrentUserService currentUser)
        {
            _productRepo = productRepo;
            _warehouseRepo = warehouseRepo;
            _stockRepo = stockRepo;
            _currentUser = currentUser;

        }

        public async Task<int> CreateAsync(CreateProductDTO dto)
        {

            var exists = await _productRepo.ExistsAsync(p => p.SKU == dto.SKU);
            if (exists)
                throw new BadRequestException("SKU already exists");

            var product = new Product
            {
                Name = dto.Name,
                SKU = dto.SKU,
                Price = dto.Price
            };

            await _productRepo.AddAsync(product);


            var stock = new ProductStock
            {
                Product = product,
                WarehouseId = _currentUser.WarehouseId!.Value,
                Quantity = dto.InitialQuantity
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

            return MapToDto(product);
        }

        public async Task<PagedResult<ProductDTO>> GetPagedAsync(PaginationParams param)
        {
            var query = _stockRepo.GetQueryable();

           

            var projectedQuery = query.Select(ps => new ProductDTO
            {
                Id = ps.Product.Id,
                Name = ps.Product.Name,
                Price = ps.Product.Price,
                SKU = ps.Product.SKU,
               
            });

            var totalCount = await projectedQuery.CountAsync();

            var products = await projectedQuery
            .OrderBy(p => p.Id)
            .Skip((param.PageNumber - 1) * param.PageSize)
             .Take(param.PageSize)
             .ToListAsync();

            return new PagedResult<ProductDTO>
            {
                Items = products,
                TotalCount = totalCount,
                PageSize = param.PageSize,
                PageNumber = param.PageNumber
            };

        }

        public async Task UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null)
                throw new NotFoundException("Product not found");

            var skuExists = await _productRepo.ExistsAsync(
                p => p.SKU == dto.SKU && p.Id != id);

            if (skuExists)
                throw new BadRequestException("SKU already exists");

            product.Name = dto.Name;
            product.SKU = dto.SKU;
            product.Price = dto.Price;

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

        private static ProductDTO MapToDto(Product product)
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