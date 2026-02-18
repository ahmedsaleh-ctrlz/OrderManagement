using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services.UsersManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepo;
        private readonly IWarehouseUserRepository _warehouseUserRepo;
        private readonly IWarehouseRepository _warehouseRepo;

        public UserManagementService(
            IUserRepository userRepo,
            IWarehouseUserRepository warehouseUserRepo,
            IWarehouseRepository warehouseRepo)
        {
            _userRepo = userRepo;
            _warehouseUserRepo = warehouseUserRepo;
            _warehouseRepo = warehouseRepo;
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
            CreateEmployeeDTO dto,
            int currentUserId,
            int? currentWarehouseId)
        {
            if (currentWarehouseId is null)
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
                WarehouseId = currentWarehouseId.Value
            };

            await _warehouseUserRepo.AddAsync(link);
            await _warehouseUserRepo.SaveChangesAsync();
        }
    }

}
