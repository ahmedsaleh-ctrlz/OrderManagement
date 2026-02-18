using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;

namespace OrderManagement.Application.Services.Warhouses
{
    public interface IWarehouseServices
    {
        Task<int> CreateAsync(CreateWarehouseDTO dto);
        Task<WarehouseDTO?> GetByIdAsync(int id);
        Task<PagedResult<WarehouseDTO>> GetPagedAsync(PaginationParams param);
        Task UpdateAsync(int id, UpdateWarehouseDTO dto);

        Task DeleteAsync(int id);


    }
}
