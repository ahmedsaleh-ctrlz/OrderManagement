using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.DTOs.Paging;

namespace OrderManagement.Application.Services.Orders
{
    public interface IOrderServices
    {
        Task<int> CreateAsync(CreateOrderDTO dto, CancellationToken ct);
        Task<OrderDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<OrderDTO>> GetPagedAsync(PaginationParams param, CancellationToken ct = default);
        Task ConfirmAsync(int id, CancellationToken ct = default);
        Task CancelAsync(int id, CancellationToken ct = default);
        Task ShipAsync(int id, CancellationToken ct = default);
        Task CompleteAsync(int orderId, CancellationToken ct = default);
    }

}
