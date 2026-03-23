using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;

namespace OrderManagement.Application.Services.Users
{
    public interface IUserServices
    {
        
        Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default);
        Task<PagedResult<UserDTO>> GetCustomersAsync(PaginationParams param, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<UserDTO?> GetByEmailAsync(string email, CancellationToken ct = default);

    }
}
