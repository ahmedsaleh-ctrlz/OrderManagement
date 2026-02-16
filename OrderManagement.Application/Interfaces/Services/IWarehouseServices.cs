using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Services
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
