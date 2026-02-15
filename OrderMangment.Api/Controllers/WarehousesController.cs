using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using OrderManagement.Application.Interfaces.Services;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseServices _warehouseService;

        public WarehousesController(IWarehouseServices warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseDTO DTO)
        {
            var id = await _warehouseService.CreateAsync(DTO);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { Id = id });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _warehouseService.GetByIdAsync(id);
            return Ok(warehouse);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param)
        {
            var result = await _warehouseService.GetPagedAsync(param);
            return Ok(result);
        }

        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWarehouseDTO DTO)
        {
            await _warehouseService.UpdateAsync(id, DTO);
            return NoContent();
        }

       
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _warehouseService.DeleteAsync(id);
            return NoContent();
        }
    }
}

