using OrderManagement.Application.DTOs.AuthDTOs;

namespace OrderManagement.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    }
}
