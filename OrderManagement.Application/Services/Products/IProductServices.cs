using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;

namespace OrderManagement.Application.Services.Products
{
    public interface IProductServices
    {
        Task<int> CreateAsync(CreateProductDTO DTO, CancellationToken ct = default);
        Task<ProductDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<ProductDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateProductDTO DTO, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
