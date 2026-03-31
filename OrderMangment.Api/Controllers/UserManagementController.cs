using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using OrderManagement.Application.Services.UsersManagement;


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
        [EndpointName("CreateWarehouseAdmin")]
        [EndpointSummary("Create warehouse admin")]
        [EndpointDescription("Creates a new warehouse admin. Only SuperAdmin can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO dto, CancellationToken ct = default)
        {
            await _service.CreateWarehouseAdminAsync(dto, ct);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin")]
        [HttpPost("create-employee")]
        [EndpointName("CreateEmployee")]
        [EndpointSummary("Create warehouse employee")]
        [EndpointDescription("Creates a new warehouse employee. Only WarehouseAdmin can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO dto, CancellationToken ct = default)
        {
            await _service.CreateEmployeeAsync(dto, ct);
            return NoContent();
        }

        [Authorize(Roles = "WarehouseAdmin,SuperAdmin")]
        [HttpGet("employees")]
        [EndpointName("GetPagedEmployees")]
        [EndpointSummary("Get paginated employees")]
        [EndpointDescription("Returns a paginated list of employees. Accessible by WarehouseAdmin and SuperAdmin.")]
        [ProducesResponseType(typeof(PagedResult<EmployeesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPagedEmployees([FromQuery] PaginationParams param, CancellationToken ct = default)
        {
            var result = await _service.GetPagedEmployees(param, ct);
            return Ok(result);
        }
    }
}
