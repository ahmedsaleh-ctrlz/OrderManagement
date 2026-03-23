using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Services.WarhouseUsers
{
    public class WarehouseUserService : IWarehouseUserService
    {
        private readonly IWarehouseUserRepository _repository;

        public WarehouseUserService(IWarehouseUserRepository repository)
        {
            _repository = repository;
        }

        public async Task AssignUserToWarehouseAsync(int userId, int warehouseId, CancellationToken ct = default)
        {
            var exists = await _repository.ExistsAsync(userId,ct);
            if (exists)
                throw new BadRequestException("User already assigned to a warehouse.");

            var entity = new WarehouseUser
            {
                UserId = userId,
                WarehouseId = warehouseId
            };

            await _repository.AddAsync(entity,ct);
            await _repository.SaveChangesAsync(ct);
        }

        public async Task<int?> GetWarehouseIdByUserAsync(int userId, CancellationToken ct = default)
        {
            var warehouseUser = await _repository.GetByUserIdAsync(userId,ct);
            return warehouseUser?.WarehouseId;
        }
    }
}
