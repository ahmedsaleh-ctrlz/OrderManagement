using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Services.Warhouses;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Services.Warhouses
{
    public class WarehouseServices : IWarehouseServices
    {
        private readonly IWarehouseRepository _repo;

        public WarehouseServices(IWarehouseRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(CreateWarehouseDTO dto,CancellationToken ct = default)
        {
            var warehouse = new Warehouse
            {
                Name = dto.Name,
                Location = dto.Location
            };

            await _repo.AddAsync(warehouse,ct);
            await _repo.SaveChangesAsync(ct);

            return warehouse.Id;
        }

        public async Task<WarehouseDTO?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var warehouse = await _repo.GetByIdAsync(id,ct);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            return WarehouseDTO.FromModel(warehouse);
        }

        public async Task<PagedResult<WarehouseDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var warehouses = await _repo.GetPagedAsync(param.PageNumber, param.PageSize,ct);
            var totalCount = await _repo.CountAsync(ct);

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
            var warehouse = await _repo.GetByIdAsync(id,ct);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            warehouse.Name = dto.Name;
            warehouse.Location = dto.Location;

            await _repo.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var warehouse = await _repo.GetByIdAsync(id,ct);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            warehouse.IsDeleted = true;

            await _repo.SaveChangesAsync(ct);
        }

       
    }
}
