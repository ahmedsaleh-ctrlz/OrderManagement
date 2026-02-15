using OrderManagement.Application.Common.Validator;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services
{
    public class WarehouseServices : IWarehouseServices
    {
        private readonly IBaseRepository<Warehouse> _repo;

        public WarehouseServices(IBaseRepository<Warehouse> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<WarehouseDTO>> GetPagedAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var warehouses = await _repo.GetPagedAsync(param.PageNumber, param.PageSize);
            var totalCount = await _repo.CountAsync();

            var mapped = warehouses.Select(MapToDto).ToList();

            return new PagedResult<WarehouseDTO>
            {
                Items = mapped,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<int> CreateAsync(CreateWarehouseDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("Warehouse name is required");

            if (string.IsNullOrWhiteSpace(dto.Location))
                throw new BadRequestException("Location is required");

            var warehouse = new Warehouse
            {
                Name = dto.Name,
                Location = dto.Location
            };

            await _repo.AddAsync(warehouse);
            await _repo.SaveChangesAsync();

            return warehouse.Id;
        }

        public async Task<WarehouseDTO> GetByIdAsync(int id)
        {
           

            var warehouse = await _repo.GetByIdAsync(id);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            return MapToDto(warehouse);
        }

        public async Task UpdateAsync(int id, UpdateWarehouseDTO dto)
        {
           

            var warehouse = await _repo.GetByIdAsync(id);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            warehouse.Name = dto.Name;
            warehouse.Location = dto.Location;

            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var warehouse = await _repo.GetByIdAsync(id);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            // Consider stock quantity check here before deletion if needed

            warehouse.IsDeleted = true;

            await _repo.SaveChangesAsync();
        }

        private static WarehouseDTO MapToDto(Warehouse warehouse)
        {
            return new WarehouseDTO
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Location = warehouse.Location
            };
        }

    }
}
