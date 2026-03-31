using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Services.ProductStocks;
using OrderManagement.Application.Services.Warhouses;

namespace OrderManagementApi.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseServices _warehouseService;
        private readonly IProductStockServices _productStockService;

        public WarehousesController(
            IWarehouseServices warehouseService,
            IProductStockServices productStockServices)
        {
            _warehouseService = warehouseService;
            _productStockService = productStockServices;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [EndpointName("CreateWarehouse")]
        [EndpointSummary("Create warehouse")]
        [EndpointDescription("Creates a new warehouse. Only SuperAdmin can perform this action.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseDTO dto, CancellationToken ct = default)
        {
            var id = await _warehouseService.CreateAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        [HttpGet("{id:int}")]
        [EndpointName("GetWarehouseById")]
        [EndpointSummary("Get warehouse by ID")]
        [EndpointDescription("Retrieves a warehouse by its ID.")]
        [ProducesResponseType(typeof(WarehouseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id, ct);

            if (warehouse == null)
                return NotFound();

            return Ok(warehouse);
        }

        [HttpGet]
        [EndpointName("GetPagedWarehouses")]
        [EndpointSummary("Get paginated warehouses")]
        [EndpointDescription("Returns a paginated list of warehouses.")]
        [ProducesResponseType(typeof(PagedResult<WarehouseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param, CancellationToken ct = default)
        {
            var result = await _warehouseService.GetPagedAsync(param, ct);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:int}")]
        [EndpointName("UpdateWarehouse")]
        [EndpointSummary("Update warehouse")]
        [EndpointDescription("Updates a warehouse. Only SuperAdmin can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWarehouseDTO dto, CancellationToken ct = default)
        {
            await _warehouseService.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id:int}")]
        [EndpointName("DeleteWarehouse")]
        [EndpointSummary("Delete warehouse")]
        [EndpointDescription("Deletes a warehouse. Only SuperAdmin can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            await _warehouseService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
