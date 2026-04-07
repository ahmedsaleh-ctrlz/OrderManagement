using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Services.Warhouses;
using OrderManagement.Domain.Entites;
using Microsoft.Extensions.Logging;

namespace OrderManagement.Application.Services.Warhouses
{
    public class WarehouseServices : IWarehouseServices
    {
        private readonly IWarehouseRepository _repo;
        private readonly ILogger<WarehouseServices> _logger;

        public WarehouseServices(IWarehouseRepository repo, ILogger<WarehouseServices> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<int> CreateAsync(CreateWarehouseDTO dto, CancellationToken ct = default)
        {
            _logger.LogInformation("Creating warehouse. Name: {Name}", dto.Name);

            var warehouse = new Warehouse
            {
                Name = dto.Name,
                Location = dto.Location
            };

            await _repo.AddAsync(warehouse, ct);
            await _repo.SaveChangesAsync(ct);

            _logger.LogInformation("Warehouse created successfully. WarehouseId: {WarehouseId}", warehouse.Id);

            return warehouse.Id;
        }

        public async Task<WarehouseDTO?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching warehouse. WarehouseId: {WarehouseId}", id);

            var warehouse = await _repo.GetByIdAsync(id, ct);
            if (warehouse is null)
            {
                _logger.LogWarning("Warehouse not found. WarehouseId: {WarehouseId}", id);
                throw new NotFoundException("Warehouse not found");
            }

            return WarehouseDTO.FromModel(warehouse);
        }

        public async Task<PagedResult<WarehouseDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching warehouses page. Page: {Page}, Size: {Size}", param.PageNumber, param.PageSize);

            var warehouses = await _repo.GetPagedAsync(param.PageNumber, param.PageSize, ct);
            var totalCount = await _repo.CountAsync(ct);

            _logger.LogInformation("Warehouses fetched. Count: {Count}", warehouses.Count);

            return new PagedResult<WarehouseDTO>
            {
                Items = warehouses.Select(WarehouseDTO.FromModel).ToList(),
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task UpdateAsync(int id, UpdateWarehouseDTO dto, CancellationToken ct = default)
        {
            _logger.LogInformation("Updating warehouse. WarehouseId: {WarehouseId}", id);

            var warehouse = await _repo.GetByIdAsync(id, ct);
            if (warehouse is null)
            {
                _logger.LogWarning("Update failed: Warehouse not found. WarehouseId: {WarehouseId}", id);
                throw new NotFoundException("Warehouse not found");
            }

            warehouse.Name = dto.Name;
            warehouse.Location = dto.Location;

            await _repo.SaveChangesAsync(ct);

            _logger.LogInformation("Warehouse updated successfully. WarehouseId: {WarehouseId}", id);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            _logger.LogInformation("Deleting warehouse. WarehouseId: {WarehouseId}", id);

            var warehouse = await _repo.GetByIdAsync(id, ct);
            if (warehouse is null)
            {
                _logger.LogWarning("Delete failed: Warehouse not found. WarehouseId: {WarehouseId}", id);
                throw new NotFoundException("Warehouse not found");
            }

            warehouse.IsDeleted = true;

            await _repo.SaveChangesAsync(ct);

            _logger.LogInformation("Warehouse deleted successfully. WarehouseId: {WarehouseId}", id);
        }
    }
}
