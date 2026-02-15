using OrderManagement.Application.Common.Validator;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IBaseRepository<User> _repo;

        public UserServices(IBaseRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize);
            var totalCount = await _repo.CountAsync();

            var mappedUsers = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email
            }).ToList();

            return new PagedResult<UserDTO>
            {
                Items = mappedUsers,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<int> CreateAsync(CreateUserDto dto)
        {
            
            PasswordValidator.Validate(dto.Password);

            var exists = await _repo.ExistsAsync(u => u.Email == dto.Email);
            if (exists)
                throw new BadRequestException("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return user.Id;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                throw new NotFoundException("User not found");

            return MapToDto(user);
        }

        public async Task UpdateAsync(int id, UpdateUserDTO dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                throw new NotFoundException("User not found");

            var emailExists = await _repo.ExistsAsync(
                u => u.Email == dto.Email && u.Id != id);

            if (emailExists)
                throw new BadRequestException("Email already exists");

            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                throw new NotFoundException("User not found");

            user.IsDeleted = true;

            await _repo.SaveChangesAsync();
        }


        

        private static UserDTO MapToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email
            };
        }
    }
}
