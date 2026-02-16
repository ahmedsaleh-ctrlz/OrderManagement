using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Domain.Entites;

public class ProductServices : IProductServices
{
    private readonly IProductRepository _productRepo;
    private readonly IWarehouseRepository _warehouseRepo;
    private readonly IProductStockRepository _stockRepo;

    public ProductServices(
        IProductRepository productRepo,
        IWarehouseRepository warehouseRepo,
        IProductStockRepository stockRepo)
    {
        _productRepo = productRepo;
        _warehouseRepo = warehouseRepo;
        _stockRepo = stockRepo;
    }

    public async Task<int> CreateAsync(CreateProductDTO dto)
    {
       
        var warehouse = await _warehouseRepo.GetByIdAsync(dto.WarehouseId);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

       
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
            WarehouseId = dto.WarehouseId,
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
        if (param.PageNumber <= 0)
            param.PageNumber = 1;

        if (param.PageSize <= 0 || param.PageSize > 100)
            param.PageSize = 10;

        var products = await _productRepo.GetPagedAsync(param.PageNumber, param.PageSize);
        var totalCount = await _productRepo.CountAsync();

        return new PagedResult<ProductDTO>
        {
            Items = products.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            PageNumber = param.PageNumber,
            PageSize = param.PageSize
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
