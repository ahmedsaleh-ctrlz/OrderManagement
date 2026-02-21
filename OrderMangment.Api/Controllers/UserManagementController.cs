using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using OrderManagement.Application.Services.UsersManagement;
using System.Security.Claims;

namespace OrderManagementApi.Controllers
{
    /// <summary>
    /// Handles administrative user management operations.
    /// </summary>
    /// <remarks>
    /// - SuperAdmin can create WarehouseAdmins.
    /// - WarehouseAdmin can create Employees within their warehouse.
    /// </remarks>
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

        /// <summary>
        /// Creates a new Warehouse Administrator.
        /// </summary>
        /// <remarks>
        /// Allowed role: SuperAdmin only.
        /// </remarks>
        /// <response code="204">Warehouse admin created successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("create-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO dto)
        {
            await _service.CreateWarehouseAdminAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// Creates a new Warehouse Employee within the current admin's warehouse.
        /// </summary>
        /// <remarks>
        /// Allowed role: WarehouseAdmin only.
        /// Employee will be automatically linked to the same warehouse.
        /// </remarks>
        /// <response code="204">Employee created successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "WarehouseAdmin")]
        [HttpPost("create-employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO dto)
        {
            await _service.CreateEmployeeAsync(dto);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpGet("employees")]
        public async Task<IActionResult> GetPagedEmployees([FromQuery] PaginationParams param)
        {
            var result = await _service.GetPagedEmployees(param);
            return Ok(result);
        }
    }
}
