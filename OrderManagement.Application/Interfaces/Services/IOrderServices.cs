using OrderManagement.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces.Services
{
    public interface IOrderServices
    {
        Task<int> CreateAsync(CreateOrderDTO dto);
        Task<OrderDTO> GetByIdAsync(int id);
        Task ConfirmAsync(int id);
        Task CancelAsync(int id);
        Task ShipAsync(int id);
        Task CompleteAsync(int orderId);
    }

}
