using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using OrderManagement.Application.Services.UsersManagement;
using System.Security.Claims;

namespace OrderManagementApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _service;

        public UserManagementController(IUserManagementService service)
        {
            _service = service;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin(CreateAdminDTO dto)
        {
            await _service.CreateWarehouseAdminAsync(dto);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin")]
        [HttpPost("create-employee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var warehouseIdClaim = User.FindFirst("warehouseId")?.Value;
            int? warehouseId = warehouseIdClaim != null ? int.Parse(warehouseIdClaim) : null;

            await _service.CreateEmployeeAsync(dto, userId, warehouseId);

            return NoContent();
        }
    }

}
