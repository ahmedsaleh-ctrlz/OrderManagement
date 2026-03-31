using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Services.Products;

namespace OrderManagementApi.Controllers
{

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

        [Authorize(Roles = "WarehouseAdmin")]
        [HttpPost]
        [EndpointName("CreateProduct")]
        [EndpointSummary("Create a new product")]
        [EndpointDescription("Creates a new product. Only warehouse admins are allowed to perform this action.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto, CancellationToken ct = default)
        {
            var id = await _productService.CreateAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [EndpointName("GetProductById")]
        [EndpointSummary("Get product by ID")]
        [EndpointDescription("Retrieves a product by its ID.")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var product = await _productService.GetByIdAsync(id, ct);
            return Ok(product);
        }

        [AllowAnonymous]
        [HttpGet]
        [EndpointName("GetPagedProducts")]
        [EndpointSummary("Get paginated products")]
        [EndpointDescription("Returns a paginated list of products.")]
        [ProducesResponseType(typeof(PagedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param, CancellationToken ct = default)
        {
            var result = await _productService.GetPagedAsync(param, ct);
            return Ok(result);
        }

        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpPut("{id:int}")]
        [EndpointName("UpdateProduct")]
        [EndpointSummary("Update a product")]
        [EndpointDescription("Updates an existing product. Only warehouse admins or super admins can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO dto, CancellationToken ct = default)
        {
            await _productService.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpDelete("{id:int}")]
        [EndpointName("DeleteProduct")]
        [EndpointSummary("Delete a product")]
        [EndpointDescription("Deletes a product. Only warehouse admins or super admins can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            await _productService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
