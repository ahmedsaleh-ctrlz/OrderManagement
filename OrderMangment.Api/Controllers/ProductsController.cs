using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Services.Products;

namespace OrderManagementApi.Controllers
{
    /// <summary>
    /// Manages product catalog operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductsController(IProductServices productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <remarks>
        /// Allowed roles: WarehouseAdmin, SuperAdmin.
        /// SKU must be unique.
        /// </remarks>
        /// <response code="201">Product created successfully</response>
        /// <response code="400">Invalid data or duplicate SKU</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO DTO)
        {
            var id = await _productService.CreateAsync(DTO);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        /// <summary>
        /// Retrieves a product by ID.
        /// </summary>
        /// <response code="200">Product found</response>
        /// <response code="404">Product not found</response>
        [Authorize(Roles = "Customer,WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// Retrieves paginated products.
        /// </summary>
        /// <response code="200">Returns paginated product list</response>
        [Authorize(Roles = "Customer,WarehouseAdmin,WarehouseEmployee,SuperAdmin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param)
        {
            var result = await _productService.GetPagedAsync(param);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <remarks>
        /// Allowed roles: WarehouseAdmin, SuperAdmin.
        /// </remarks>
        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO DTO)
        {
            await _productService.UpdateAsync(id, DTO);
            return NoContent();
        }

        /// <summary>
        /// Soft deletes a product.
        /// </summary>
        /// <remarks>
        /// Allowed roles: WarehouseAdmin, SuperAdmin.
        /// </remarks>
        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
