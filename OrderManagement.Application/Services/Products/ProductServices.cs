using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Global;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using System.Text.Json;


namespace OrderManagement.Application.Services.Products
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepo;
        private readonly IWarehouseRepository _warehouseRepo;
        private readonly IProductStockRepository _stockRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IDistributedCache _cache;
        


        public ProductServices(
            IProductRepository productRepo,
            IWarehouseRepository warehouseRepo,
            IProductStockRepository stockRepo,
            ICurrentUserService currentUser,
            IDistributedCache cache)
        {
            _productRepo = productRepo;
            _warehouseRepo = warehouseRepo;
            _stockRepo = stockRepo;
            _currentUser = currentUser;
            _cache = cache;

        }

        public async Task<int> CreateAsync(CreateProductDTO dto, CancellationToken ct = default)
        {

            var exists = await _productRepo.ExistsAsync(p => p.SKU == dto.SKU,ct);
            if (exists)
                throw new BadRequestException("SKU already exists");

            var product = new Product
            {
                Name = dto.Name,
                SKU = dto.SKU,
                Price = dto.Price
            };

            await _productRepo.AddAsync(product,ct);


            var stock = new ProductStock
            {
                Product = product,
                WarehouseId = _currentUser.WarehouseId!.Value,
                Quantity = dto.InitialQuantity
            };

            await _stockRepo.AddAsync(stock,ct);


            await _productRepo.SaveChangesAsync(ct);

            return product.Id;
        }

        public async Task<ProductDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var product = await _productRepo.GetByIdAsync(id,ct);
            if (product is null)
                throw new NotFoundException("Product not found");

            return ProductDTO.FromModel(product);
        }

        public async Task<PagedResult<ProductDTO>> GetPagedAsync(
        PaginationParams param,
        CancellationToken ct = default)
        {
            // To Make Cache Invalidate When Data Changes, We Use A Versioning System For Cache Keys
            var version = await _cache.GetStringAsync("products_version", ct) ?? "1";

           
            var cacheKey = $"products_v{version}_p{param.PageNumber}_s{param.PageSize}";

            
            var shouldCache = param.PageNumber <= 5;

            if (shouldCache)
            {
                var cacheData = await _cache.GetStringAsync(cacheKey, ct);

                if (cacheData is not null)
                {
                    return JsonSerializer.Deserialize<PagedResult<ProductDTO>>(cacheData)!;
                }
            }

           
            var query = _stockRepo.GetQueryable();

            

            var projectedQuery = query.Select(ProductDTO.Selector);

           
           

            var totalCount = await projectedQuery.CountAsync(ct);

            var products = await projectedQuery
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync(ct);

            var data = new PagedResult<ProductDTO>
            {
                Items = products,
                TotalCount = totalCount,
                PageSize = param.PageSize,
                PageNumber = param.PageNumber
            };

            if (shouldCache)
            {
                var serializedData = JsonSerializer.Serialize(data);

                await _cache.SetStringAsync(
                    cacheKey,
                    serializedData,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) 
                    },
                    ct);
            }

            return data;
        }

        public async Task UpdateAsync(int id, UpdateProductDTO dto, CancellationToken ct = default)
        {
            var product = await _productRepo.GetByIdAsync(id,ct);
            if (product is null)
                throw new NotFoundException("Product not found");

            var skuExists = await _productRepo.ExistsAsync(
                p => p.SKU == dto.SKU && p.Id != id,ct);

            if (skuExists)
                throw new BadRequestException("SKU already exists");

            product.Name = dto.Name;
            product.SKU = dto.SKU;
            product.Price = dto.Price;

            await _productRepo.SaveChangesAsync(ct);

            await _cache.SetStringAsync(
            "products_version",
            Guid.NewGuid().ToString(),
            ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var product = await _productRepo.GetByIdAsync(id,ct);
            if (product is null)
                throw new NotFoundException("Product not found");

            product.IsDeleted = true;

            await _productRepo.SaveChangesAsync(ct);
            await _cache.SetStringAsync(
            "products_version",
            Guid.NewGuid().ToString(),
            ct);
        }

        
    }
}