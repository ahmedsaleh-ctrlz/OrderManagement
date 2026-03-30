using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Services.Auth;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IWarehouseUserRepository _warehouseUserRepo;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepo;

    public AuthService(
        IUserRepository userRepo,
        IWarehouseUserRepository warehouseUserRepo,
        IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepo)
    {
        _userRepo = userRepo;
        _warehouseUserRepo = warehouseUserRepo;
        _configuration = configuration;
        _refreshTokenRepo = refreshTokenRepo;   
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto, CancellationToken ct = default)
    {
        var exists = await _userRepo.ExistsAsync(u => u.Email == dto.Email,ct);
        if (exists)
            throw new BadRequestException("Email already exists");
        

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Customer
        };

        await _userRepo.AddAsync(user,ct);
        await _userRepo.SaveChangesAsync(ct);

        return await GenerateTokenAsync(user);
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto, CancellationToken ct = default)
    {
        var user = await _userRepo.FirstOrDefaultAsync(u => u.Email == dto.Email,ct);

        if (user is null ||
            !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new BadRequestException("Invalid credentials");
        }

        return await GenerateTokenAsync(user,ct);
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        var hashedToken = HashToken(request.RefreshToken);
        var tokenRecord = await _refreshTokenRepo.GetByTokenHashAsync(hashedToken, ct);

        if (tokenRecord is null || tokenRecord.ExpiryDate < DateTime.UtcNow)
        {
            throw new BadRequestException("Invalid or expired refresh token");
        }

        if (tokenRecord!.RevokedAt != null)
        {
            
            throw new BadRequestException("Invalid refresh token");
        }

       

        var user = await _userRepo.GetByIdAsync(tokenRecord.UserId, ct);
        if (user == null)
        {
            throw new BadRequestException("User not found");
        }

        // Invalidate the old refresh token
        
        tokenRecord.RevokedAt = DateTime.UtcNow;
        
        var NewTokens = await GenerateTokenAsync(user, ct);
        tokenRecord.ReplacedByToken = HashToken(NewTokens.RefreshToken);
        await _refreshTokenRepo.SaveChangesAsync(ct);

        return NewTokens;
    }

    public async Task LogoutAsync(LogoutRequest request, CancellationToken ct)
    {
       
        var hashedToken = HashToken(request.RefreshToken);

       
        var tokenRecord = await _refreshTokenRepo
            .GetByTokenHashAsync(hashedToken, ct);

     
        if (tokenRecord is null)
            return;
 
        if (tokenRecord.RevokedAt != null)
            return;

        tokenRecord.RevokedAt = DateTime.UtcNow;    
        await _refreshTokenRepo.SaveChangesAsync(ct);
    }

    private async Task<AuthResponseDTO> GenerateTokenAsync(User user, CancellationToken ct = default)
    {
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
                await _warehouseUserRepo.GetByUserIdAsync(user.Id,ct);

            if (warehouseUser != null)
            {
                claims.Add(new Claim(
                    "warehouseId",
                    warehouseUser.WarehouseId.ToString()));
            }
        }

        var secret = _configuration["JWT_SECRET_KEY"];
        if (string.IsNullOrEmpty(secret))
            throw new Exception("JWT_SECRET is not configured.");

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
