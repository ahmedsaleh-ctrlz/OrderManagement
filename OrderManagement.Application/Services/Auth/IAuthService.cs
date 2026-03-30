using OrderManagement.Application.DTOs.AuthDTOs;

namespace OrderManagement.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto, CancellationToken ct = default);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto, CancellationToken ct = default);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct);

        Task LogoutAsync(LogoutRequest request, CancellationToken ct);
    }
}
