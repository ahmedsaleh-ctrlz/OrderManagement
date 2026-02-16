using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.DTOs.ProductStockDTOs;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Domain.Entites;

namespace OrderManagement.Application.Interfaces.Services
{
    public interface IProductStockServices
    {

        
        Task AddStockAsync(AddStockDTO DTO);
        Task DeductStockAsync(DeductStockDTO DTO);
        Task<int> GetQuantityAsync(ProductQuantityDTO DTO);
        Task<List<WarehouseStockDTO>> GetStocksByWarehouseAsync(int warehouseId);

    }
}
