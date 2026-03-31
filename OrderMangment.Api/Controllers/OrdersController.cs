using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.Services.Orders;

namespace OrderManagementApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderService;

        public OrdersController(IOrderServices orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [EndpointName("CreateOrder")]
        [EndpointSummary("Create a new order")]
        [EndpointDescription("Creates a new order for the authenticated customer.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto, CancellationToken ct)
        {
            var id = await _orderService.CreateAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        [HttpGet]
        [EndpointName("GetPagedOrders")]
        [EndpointSummary("Get paginated orders")]
        [EndpointDescription("Returns a paginated list of orders for the authenticated user.")]
        [ProducesResponseType(typeof(PagedResult<OrderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PaginationParams param, CancellationToken ct = default)
        {
            var orders = await _orderService.GetPagedAsync(param, ct);
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        [EndpointName("GetOrderById")]
        [EndpointSummary("Get order by ID")]
        [EndpointDescription("Retrieves a specific order by its ID.")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var order = await _orderService.GetByIdAsync(id, ct);
            return Ok(order);
        }

        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/confirm")]
        [EndpointName("ConfirmOrder")]
        [EndpointSummary("Confirm an order")]
        [EndpointDescription("Confirms an order. Only warehouse staff or admins can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Confirm(int id, CancellationToken ct = default)
        {
            await _orderService.ConfirmAsync(id, ct);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/cancel")]
        [EndpointName("CancelOrder")]
        [EndpointSummary("Cancel an order")]
        [EndpointDescription("Cancels an order. Only warehouse staff or admins can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct = default)
        {
            await _orderService.CancelAsync(id, ct);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/ship")]
        [EndpointName("ShipOrder")]
        [EndpointSummary("Ship an order")]
        [EndpointDescription("Marks an order as shipped. Only warehouse staff or admins can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Ship(int id, CancellationToken ct = default)
        {
            await _orderService.ShipAsync(id, ct);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/complete")]
        [EndpointName("CompleteOrder")]
        [EndpointSummary("Complete an order")]
        [EndpointDescription("Marks an order as completed. Only warehouse staff or admins can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Complete(int id, CancellationToken ct = default)
        {
            await _orderService.CompleteAsync(id, ct);
            return NoContent();
        }
    }
}
