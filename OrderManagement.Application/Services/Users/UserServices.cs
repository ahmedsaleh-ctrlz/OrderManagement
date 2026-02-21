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

        public async Task<PagedResult<UserDTO>> GetPagedAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize);
            var totalCount = await _repo.CountAsync();

            var mappedUsers = users.Select(MapToDto).ToList();

            return new PagedResult<UserDTO>
            {
                Items = mappedUsers,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }

        public async Task<PagedResult<UserDTO>> GetCustomersAsync(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;

            var users = await _repo.GetPagedAsync(param.PageNumber, param.PageSize, u => u.Role == UserRole.Customer);
            var totalCount = await _repo.CountAsync(u => u.Role == UserRole.Customer);

            var mappedUsers = users.Select(MapToDto).ToList();

            return new PagedResult<UserDTO>
            {
                Items = mappedUsers,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
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
        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var user = await _repo.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
                throw new NotFoundException("User not found");
            return MapToDto(user);
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
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
