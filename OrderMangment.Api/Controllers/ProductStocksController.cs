using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.ProductStockDTOs;
using OrderManagement.Application.Interfaces.Services;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStocksController : ControllerBase
    {
        private readonly IProductStockServices _stockService;

        public ProductStocksController(IProductStockServices stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddStock([FromBody] AddStockDTO dto)
        {
            await _stockService.AddStockAsync(dto);
            return NoContent();
        }

        [HttpPost("deduct")]
        public async Task<IActionResult> DeductStock([FromBody] DeductStockDTO dto)
        {
            await _stockService.DeductStockAsync(dto);
            return NoContent();
        }

     
        [HttpGet("quantity")]
        public async Task<IActionResult> GetQuantity([FromQuery] ProductQuantityDTO DTO)
        {
            var quantity = await _stockService
                .GetQuantityAsync(DTO);

            return Ok(new { quantity });
        }
    }
}
