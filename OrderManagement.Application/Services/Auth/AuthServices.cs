using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Services.Auth;
using OrderManagement.Application.Services.Users;
using OrderManagement.Application.Services.WarhouseUsers;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserServices _userSevices;
    private readonly IWarehouseUserService _warehouseUserServices;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserServices userServices,
        IWarehouseUserService warehouseUserService,
        IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepo,
        ILogger<AuthService> logger)
    {
        _userSevices = userServices;
        _warehouseUserServices = warehouseUserService;
        _configuration = configuration;
        _refreshTokenRepo = refreshTokenRepo;
        _logger = logger;

    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto, CancellationToken ct = default)
    {
        _logger.LogTrace("Starting registration for {Email}", dto.Email);

        try
        {
            var newUser = await _userSevices.AddUserAsync(dto, ct);

            _logger.LogInformation("User registered successfully. UserId: {UserId}, Email: {Email}", newUser.Id, newUser.Email);

            return await GenerateTokenAsync(newUser, ct);
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning(
                "Registration failed: {Message}, Email: {Email}",
                ex.Message,
                dto.Email);

            throw;
        }
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto, CancellationToken ct = default)
    {
        _logger.LogTrace("Login attempt for {Email}", dto.Email);

        var user = await _userSevices.VerfiyLoginAsync(dto, ct);

        if (user is null)
        {
            _logger.LogWarning("Failed login attempt for {Email}", dto.Email);
            throw new BadRequestException("Invalid Email Or Password");
        }

        _logger.LogInformation("User logged in successfully. UserId: {UserId}", user.Id);

        return await GenerateTokenAsync(user, ct);
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        _logger.LogTrace("Refresh token attempt");

        var hashedToken = HashToken(request.RefreshToken);
        var tokenRecord = await _refreshTokenRepo.GetByTokenHashAsync(hashedToken, ct);

        if (tokenRecord is null)
        {
            _logger.LogWarning("Refresh token not found");
            throw new BadRequestException("Invalid refresh token");
        }

        if (tokenRecord.ExpiryDate < DateTime.UtcNow)
        {
            _logger.LogWarning("Expired refresh token for UserId: {UserId}", tokenRecord.UserId);
            throw new BadRequestException("Invalid refresh token");
        }

        if (tokenRecord.RevokedAt != null)
        {
            _logger.LogWarning("Revoked refresh token reuse attempt for UserId: {UserId}", tokenRecord.UserId);
            throw new BadRequestException("Invalid refresh token");
        }

        var user = await _userSevices.GetUserAsync(tokenRecord.UserId, ct);

        tokenRecord.RevokedAt = DateTime.UtcNow;

        var newTokens = await GenerateTokenAsync(user, ct);

        tokenRecord.ReplacedByToken = HashToken(newTokens.RefreshToken);
        await _refreshTokenRepo.SaveChangesAsync(ct);

        _logger.LogInformation("Refresh token succeeded for UserId: {UserId}", user.Id);

        return newTokens;
    }

    public async Task LogoutAsync(LogoutRequest request, CancellationToken ct)
    {
        _logger.LogTrace("Logout attempt");

        var hashedToken = HashToken(request.RefreshToken);

        var tokenRecord = await _refreshTokenRepo
            .GetByTokenHashAsync(hashedToken, ct);

        if (tokenRecord is null)
        {
            _logger.LogWarning("Logout attempt with invalid token");
            return;
        }

        if (tokenRecord.RevokedAt != null)
        {
            _logger.LogWarning("Logout attempt with already revoked token. UserId: {UserId}", tokenRecord.UserId);
            return;
        }

        tokenRecord.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepo.SaveChangesAsync(ct);

        _logger.LogInformation("User logged out successfully. UserId: {UserId}", tokenRecord.UserId);
    }

    private async Task<AuthResponseDTO> GenerateTokenAsync(User user, CancellationToken ct = default)
    {
        _logger.LogDebug("Generating token for UserId: {UserId}", user.Id);

        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Role, user.Role.ToString())
    };

        if (user.Role == UserRole.WarehouseAdmin ||
            user.Role == UserRole.WarehouseEmployee)
        {
            var warehouseUser =
                await _warehouseUserServices.GetByUserIdAsync(user.Id, ct);

            if (warehouseUser != null)
            {
                claims.Add(new Claim(
                    "warehouseId",
                    warehouseUser.WarehouseId.ToString()));
            }
        }

        var secret = _configuration["JWT_SECRET_KEY"];
        if (string.IsNullOrEmpty(secret))
        {
            _logger.LogCritical("JWT_SECRET_KEY is missing in configuration");
            throw new Exception("JWT_SECRET is not configured.");
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(15);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        var refreshToken = GenerateRefreshToken();
        var hashedToken = HashToken(refreshToken);

        var refreshTokenRecord = new RefreshToken
        {
            TokenHash = hashedToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepo.AddAsync(refreshTokenRecord, ct);
        await _refreshTokenRepo.SaveChangesAsync(ct);

        _logger.LogInformation("Token generated successfully for UserId: {UserId}", user.Id);

        return new AuthResponseDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
        };
    }
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
    private string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

   
}
