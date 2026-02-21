using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Services.ProductStocks;
using OrderManagement.Application.Services.Warhouses;

namespace OrderManagementApi.Controllers
{
    /// <summary>
    /// Manages warehouse operations and warehouse stock visibility.
    /// </summary>
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

        /// <summary>
        /// Creates a new warehouse.
        /// </summary>
        /// <remarks>
        /// Allowed role: SuperAdmin only.
        /// </remarks>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseDTO DTO)
        {
            var id = await _warehouseService.CreateAsync(DTO);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        /// <summary>
        /// Retrieves warehouse by ID.
        /// </summary>
   
        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            return Ok(warehouse);
        }

        /// <summary>
        /// Retrieves paginated warehouses.
        /// </summary>
    
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param)
        {
            var result = await _warehouseService.GetPagedAsync(param);
            return Ok(result);
        }

        /// <summary>
        /// Updates warehouse information.
        /// </summary>
        /// <remarks>
        /// Allowed role: SuperAdmin.
        /// </remarks>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWarehouseDTO DTO)
        {
            await _warehouseService.UpdateAsync(id, DTO);
            return NoContent();
        }

        /// <summary>
        /// Soft deletes a warehouse.
        /// </summary>
        /// <remarks>
        /// Allowed role: SuperAdmin.
        /// </remarks>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            await _warehouseService.DeleteAsync(id);
            return NoContent();
        }

    }
}
