using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;

namespace OrderManagement.Application.Services.Users
{
    public interface IUserServices
    {
        
        Task<UserDTO?> GetByIdAsync(int id);
        Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param);
        Task<PagedResult<UserDTO>> GetCustomersAsync(PaginationParams param);
        Task UpdateAsync(int id, UpdateUserDTO dto);
        Task DeleteAsync(int id);
        Task<UserDTO?> GetByEmailAsync(string email);

    }
}
