using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Domain.Entites;

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
        Task<User> AddUserAsync(RegisterDTO dto, CancellationToken ct = default);
        Task<User> VerfiyLoginAsync(LoginDTO dto, CancellationToken ct = default);
        Task<User> GetUserAsync(int id, CancellationToken ct = default);

    }
}
