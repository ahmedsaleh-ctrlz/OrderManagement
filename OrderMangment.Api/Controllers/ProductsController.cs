using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.ProductDTOs;
using OrderManagement.Application.Interfaces.Services;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductsController(IProductServices productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO DTO)
        {
            var id = await _productService.CreateAsync(DTO);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param)
        {
            var result = await _productService.GetPagedAsync(param);
            return Ok(result);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO DTO)
        {
            await _productService.UpdateAsync(id, DTO);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}

