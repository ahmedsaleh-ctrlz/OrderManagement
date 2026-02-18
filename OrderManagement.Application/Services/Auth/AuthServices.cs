using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.Common.Validator;
using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Services.Auth;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IWarehouseUserRepository _warehouseUserRepo;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepo,
        IWarehouseUserRepository warehouseUserRepo,
        IConfiguration configuration)
    {
        _userRepo = userRepo;
        _warehouseUserRepo = warehouseUserRepo;
        _configuration = configuration;
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
    {
        var exists = await _userRepo.ExistsAsync(u => u.Email == dto.Email);
        if (exists)
            throw new BadRequestException("Email already exists");
        PasswordValidator.Validate(dto.Password);

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Customer
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        return await GenerateTokenAsync(user);
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        var user = await _userRepo.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null ||
            !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new BadRequestException("Invalid credentials");
        }

        return await GenerateTokenAsync(user);
    }

    private async Task<AuthResponseDTO> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        if (user.Role == UserRole.WarehouseAdmin ||
            user.Role == UserRole.WarehouseEmployee)
        {
            var warehouseUser =
                await _warehouseUserRepo.GetByUserIdAsync(user.Id);

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

        return new AuthResponseDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
