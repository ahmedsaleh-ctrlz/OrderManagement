using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.Services.Orders;

namespace OrderManagementApi.Controllers
{
    /// <summary>
    /// Handles order lifecycle operations including creation, retrieval,
    /// and status transitions.
    /// </summary>
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

        /// <summary>
        /// Creates a new order (Customer only).
        /// </summary>
        /// <remarks>
        /// The order will be created with initial status "Pending".
        /// Stock validation is performed internally.
        /// </remarks>
        /// <response code="201">Order created successfully</response>
        /// <response code="400">Invalid request or insufficient stock</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            var id = await _orderService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        /// <summary>
        /// Retrieves paginated orders.
        /// Ownership rules apply based on role.
        /// </summary>
        /// <response code="200">Returns paginated list of orders</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PaginationParams param)
        {
            var orders = await _orderService.GetPagedAsync(param);
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves a specific order by ID.
        /// Ownership validation is enforced.
        /// </summary>
        /// <response code="200">Order found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden - ownership violation</response>
        /// <response code="404">Order not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }

        /// <summary>
        /// Confirms an order.
        /// </summary>
        /// <remarks>
        /// Allowed roles: WarehouseAdmin, WarehouseEmployee, SuperAdmin.
        /// Stock will be deducted upon confirmation.
        /// </remarks>
        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/confirm")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Confirm(int id)
        {
            await _orderService.ConfirmAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <remarks>
        /// Stock will be restored if already deducted.
        /// </remarks>
        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id)
        {
            await _orderService.CancelAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Marks an order as shipped.
        /// </summary>
        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/ship")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Ship(int id)
        {
            await _orderService.ShipAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Marks an order as completed.
        /// </summary>
        [Authorize(Roles = "WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpPut("{id:int}/complete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Complete(int id)
        {
            await _orderService.CompleteAsync(id);
            return NoContent();
        }
    }
}
