using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.DTOs.Paging;

namespace OrderManagement.Application.Services.Orders
{
    public interface IOrderServices
    {
        Task<int> CreateAsync(CreateOrderDTO dto);
        Task<OrderDTO> GetByIdAsync(int id);
        Task<PagedResult<OrderDTO>> GetPagedAsync(PaginationParams param);
        Task ConfirmAsync(int id);
        Task CancelAsync(int id);
        Task ShipAsync(int id);
        Task CompleteAsync(int orderId);
    }

}
