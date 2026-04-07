using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using OrderManagement.Application.Interfaces.Global;


namespace OrderManagement.Application.Services.Users
{

    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<UserServices> _logger;

        public UserServices(IUserRepository repo,ICurrentUserService currentUser ,ILogger<UserServices> logger)
        {
            _repo = repo;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching users page. Page: {Page}, Size: {Size}", param.PageNumber, param.PageSize);

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize, null, ct);
            var totalCount = await _repo.CountAsync(null, ct);

            _logger.LogInformation("Users fetched. Count: {Count}", users.Count);

            return new PagedResult<UserDTO>
            {
                Items = users.Select(UserDTO.FromModel).ToList(),
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<PagedResult<UserDTO>> GetCustomersAsync(PaginationParams param, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching customers page. Page: {Page}, Size: {Size}", param.PageNumber, param.PageSize);

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize, u => u.Role == UserRole.Customer, ct);
            var totalCount = await _repo.CountAsync(u => u.Role == UserRole.Customer, ct);

            _logger.LogInformation("Customers fetched. Count: {Count}", users.Count);

            return new PagedResult<UserDTO>
            {
                Items = users.Select(UserDTO.FromModel).ToList(),
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<UserDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching user. UserId: {UserId}", id);

            var user = await _repo.GetByIdAsync(id, ct);
            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", id);
                throw new NotFoundException("User not found");
            }

            return UserDTO.FromModel(user);
        }

        public async Task UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct = default)
        {
            _logger.LogInformation("Updating user. UserId: {UserId}", id);

            var user = await _repo.GetByIdAsync(id, ct);
            if (user is null)
            {
                _logger.LogWarning("Update failed: User not found. UserId: {UserId}", id);
                throw new NotFoundException("User not found");
            }

            var emailExists = await _repo.ExistsAsync(
                u => u.Email == dto.Email && u.Id != id, ct);

            if (emailExists)
            {
                _logger.LogWarning("Update failed: Email already exists. Email: {Email}", dto.Email);
                throw new BadRequestException("Email already exists");
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _repo.SaveChangesAsync(ct);

            _logger.LogInformation("User updated successfully. UserId: {UserId}", id);
        }

        public async Task<UserDTO> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching user by email.");

            var user = await _repo.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user is null)
            {
                _logger.LogWarning("User not found by email.");
                throw new NotFoundException("User not found");
            }

            return UserDTO.FromModel(user);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            _logger.LogInformation("Deleting user. UserId: {UserId}", id);

            var user = await _repo.GetByIdAsync(id, ct);
            if (user is null)
            {
                _logger.LogWarning("Delete failed: User not found. UserId: {UserId}", id);
                throw new NotFoundException("User not found");
            }

            user.IsDeleted = true;
            await _repo.SaveChangesAsync(ct);

            _logger.LogInformation("User {currentuserId} deleted successfully. UserId: {UserId}",_currentUser.UserId,id);
        }

        public async Task<User> AddUserAsync(RegisterDTO dto, CancellationToken ct = default)
        {
            _logger.LogInformation("Creating user. Email: {Email}", dto.Email);

            var exists = await _repo.ExistsAsync(u => u.Email == dto.Email, ct);
            if (exists)
            {
                _logger.LogWarning("User creation failed: Email already exists. Email: {Email}", dto.Email);
                throw new BadRequestException("Email already exists");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Customer
            };

            await _repo.AddAsync(user, ct);
            await _repo.SaveChangesAsync(ct);

            _logger.LogInformation("User created successfully. UserId: {UserId}", user.Id);

            return user;
        }

        public async Task<User> VerfiyLoginAsync(LoginDTO dto, CancellationToken ct = default)
        {
            var user = await _repo.FirstOrDefaultAsync(u => u.Email == dto.Email, ct);

            if (user is null ||
                !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for {Email}", dto.Email);
                return null;
            }

            _logger.LogInformation("User login verified. UserId: {UserId}", user.Id);

            return user;
        }

        public async Task<User> GetUserAsync(int id, CancellationToken ct = default)
        {
            _logger.LogDebug("Fetching user entity. UserId: {UserId}", id);

            var user = await _repo.GetByIdAsync(id, ct);
            if (user is null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", id);
                throw new NotFoundException("User not found");
            }

            return user;
        }
    }
}
