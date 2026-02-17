using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Services
{
    public interface IUserManagementService
    {
        Task CreateWarehouseAdminAsync(CreateAdminDTO dto);

        Task CreateEmployeeAsync(CreateEmployeeDTO dto, int currentUserId, int? currentWarehouseId);
    }
}
