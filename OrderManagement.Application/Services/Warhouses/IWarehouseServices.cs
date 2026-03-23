using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;

namespace OrderManagement.Application.Services.Warhouses
{
    public interface IWarehouseServices
    {
        Task<int> CreateAsync(CreateWarehouseDTO dto, CancellationToken ct = default);
        Task<WarehouseDTO?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<WarehouseDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateWarehouseDTO dto, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);


    }
}
