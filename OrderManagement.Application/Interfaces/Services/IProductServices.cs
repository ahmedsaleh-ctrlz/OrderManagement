using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Services
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
