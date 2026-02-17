using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Domain.Entites;


public class WarehouseUserService : IWarehouseUserService
{
    private readonly IWarehouseUserRepository _repository;

    public WarehouseUserService(IWarehouseUserRepository repository)
    {
        _repository = repository;
    }

    public async Task AssignUserToWarehouseAsync(int userId, int warehouseId)
    {
        var exists = await _repository.ExistsAsync(userId);
        if (exists)
            throw new BadRequestException("User already assigned to a warehouse.");

        var entity = new WarehouseUser
        {
            UserId = userId,
            WarehouseId = warehouseId
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task<int?> GetWarehouseIdByUserAsync(int userId)
    {
        var warehouseUser = await _repository.GetByUserIdAsync(userId);
        return warehouseUser?.WarehouseId;
    }
}
