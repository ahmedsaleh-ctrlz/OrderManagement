using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<int> CreateAsync(CreateUserDto dto);
        Task<UserDTO?> GetByIdAsync(int id);
        Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param);
        Task UpdateAsync(int id, UpdateUserDTO dto);
        Task DeleteAsync(int id);
        Task<UserDTO?> GetByEmailAsync(string email);

    }
}
