using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services.UsersManagement
{
    public interface IUserManagementService
    {
        Task CreateWarehouseAdminAsync(CreateAdminDTO dto,CancellationToken ct = default);

        Task CreateEmployeeAsync(CreateEmployeeDTO dto, CancellationToken ct = default);

        Task<PagedResult<EmployeesDTO>> GetPagedEmployees(PaginationParams param, CancellationToken ct = default);
        
    }
}
