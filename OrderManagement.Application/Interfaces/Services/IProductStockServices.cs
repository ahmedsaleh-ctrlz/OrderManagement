using OrderManagement.Application.DTOs.ProductStockDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Services
{
    public interface IProductStockServices
    {
        Task AddStockAsync(AddStockDTO DTO);
        Task DeductStockAsync(DeductStockDTO DTO);
        Task<int> GetQuantityAsync(ProductQuantityDTO DTO);
    }
}
