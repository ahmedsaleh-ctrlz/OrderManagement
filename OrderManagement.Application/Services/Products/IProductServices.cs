using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;

namespace OrderManagement.Application.Services.Products
{
    public interface IProductServices
    {
        Task<int> CreateAsync(CreateProductDTO DTO);
        Task<ProductDTO> GetByIdAsync(int id);
        Task<PagedResult<ProductDTO>> GetPagedAsync(PaginationParams param);
        Task UpdateAsync(int id, UpdateProductDTO DTO);
        Task DeleteAsync(int id);
    }
}
