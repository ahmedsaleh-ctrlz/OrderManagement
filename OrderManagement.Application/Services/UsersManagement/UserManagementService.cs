using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Global;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;


namespace OrderManagement.Application.Services.UsersManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepo;
        private readonly IWarehouseUserRepository _warehouseUserRepo;
        private readonly IWarehouseRepository _warehouseRepo;
        private readonly ICurrentUserService _currentUser;

        public UserManagementService(
            IUserRepository userRepo,
            IWarehouseUserRepository warehouseUserRepo,
            IWarehouseRepository warehouseRepo,
            ICurrentUserService currentUser)
        {
            _userRepo = userRepo;
            _warehouseUserRepo = warehouseUserRepo;
            _warehouseRepo = warehouseRepo;
            _currentUser = currentUser;
        }


        public async Task CreateWarehouseAdminAsync(CreateAdminDTO dto)
        {
            var warehouse = await _warehouseRepo.GetByIdAsync(dto.WarehouseId);
            if (warehouse is null)
                throw new NotFoundException("Warehouse not found");

            var exists = await _userRepo.ExistsAsync(u => u.Email == dto.Email);
            if (exists)
                throw new BadRequestException("Email already exists");

            var admin = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.WarehouseAdmin
            };

            await _userRepo.AddAsync(admin);
            await _userRepo.SaveChangesAsync();

            var link = new WarehouseUser
            {
                UserId = admin.Id,
                WarehouseId = dto.WarehouseId
            };

            await _warehouseUserRepo.AddAsync(link);
            await _warehouseUserRepo.SaveChangesAsync();
        }


        public async Task CreateEmployeeAsync(
            CreateEmployeeDTO dto)
        {
            if (_currentUser.WarehouseId is null)
                throw new BadRequestException("Admin is not linked to a warehouse");

            var exists = await _userRepo.ExistsAsync(u => u.Email == dto.Email);
            if (exists)
                throw new BadRequestException("Email already exists");

            var employee = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.WarehouseEmployee
            };

            await _userRepo.AddAsync(employee);
            await _userRepo.SaveChangesAsync();

            var link = new WarehouseUser
            {
                UserId = employee.Id,
                WarehouseId = _currentUser.WarehouseId.Value
            };

            await _warehouseUserRepo.AddAsync(link);
            await _warehouseUserRepo.SaveChangesAsync();
        }


        public async Task<PagedResult<EmployeesDTO>> GetPagedEmployees(PaginationParams param)
        {
            if (param.PageNumber <= 0)
                param.PageNumber = 1;

            if (param.PageSize <= 0 || param.PageSize > 100)
                param.PageSize = 10;
            var query = _warehouseUserRepo.GetQueryable();
            // SuperAdmin can see all employees, WarehouseAdmin can see only employees from their warehouse
            if (_currentUser.Role == "WarehouseAdmin")
            {
                query = query
                    .Where(x => x.WarehouseId == _currentUser.WarehouseId &&
                                x.User.Role == UserRole.WarehouseEmployee);
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderBy(x => x.User.FullName)
                .Skip((param.PageNumber - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(x => new EmployeesDTO
                {
                    Id = x.User.Id,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    WarehouseName = x.Warehouse.Name
                })
                .ToListAsync();

            return new PagedResult<EmployeesDTO>
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize
            };
        }
    }

}
