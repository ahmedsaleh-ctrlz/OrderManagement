using OrderManagement.Application.Common.Validator;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;

using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;


namespace OrderManagement.Application.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repo;

        public UserServices(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize,null,ct);
            var totalCount = await _repo.CountAsync(null,ct);

            var mappedUsers = users.Select(UserDTO.FromModel).ToList();

            return new PagedResult<UserDTO>
            {
                Items = mappedUsers,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<PagedResult<UserDTO>> GetCustomersAsync(PaginationParams param, CancellationToken ct = default)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize, u => u.Role == UserRole.Customer,ct);
            var totalCount = await _repo.CountAsync(u => u.Role == UserRole.Customer,ct);

            var mappedUsers = users.Select(UserDTO.FromModel).ToList();

            return new PagedResult<UserDTO>
            {
                Items = mappedUsers,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }



        public async Task<UserDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _repo.GetByIdAsync(id,ct);
            return user is null ? throw new NotFoundException("User not found") : UserDTO.FromModel(user);
        }

        public async Task UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct = default)
        {
            var user = await _repo.GetByIdAsync(id,ct);
            if (user is null)
                throw new NotFoundException("User not found");

            var emailExists = await _repo.ExistsAsync(
                u => u.Email == dto.Email && u.Id != id,ct);

            if (emailExists)
                throw new BadRequestException("Email already exists");

            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _repo.SaveChangesAsync(ct);
        }
        public async Task<UserDTO> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _repo.FirstOrDefaultAsync(u => u.Email == email,ct);
            if (user is null)
                throw new NotFoundException("User not found");
            return UserDTO.FromModel(user);
        }
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {

            var user = await _repo.GetByIdAsync(id,ct);
            if (user is null)
                throw new NotFoundException("User not found");
            user.IsDeleted = true;
            await _repo.SaveChangesAsync(ct);
        }

    }
}
