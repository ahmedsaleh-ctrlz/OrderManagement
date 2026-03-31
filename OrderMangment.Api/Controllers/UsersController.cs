using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.Services.Users;

namespace OrderManagementApi.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IAuthorizationService _authorizationService;

        public UsersController(
            IUserServices userService,
            IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id:int}")]
        [EndpointName("GetUserById")]
        [EndpointSummary("Get user by ID")]
        [EndpointDescription("Retrieves a user by ID. Access is restricted based on ownership policy.")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var user = await _userService.GetByIdAsync(id, ct);

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                user,
                "UserOwnershipPolicy");

            if (!authResult.Succeeded)
                return Forbid();

            return Ok(user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("all")]
        [EndpointName("GetAllUsersPaged")]
        [EndpointSummary("Get all users")]
        [EndpointDescription("Returns a paginated list of all users. Only SuperAdmin can access this endpoint.")]
        [ProducesResponseType(typeof(PagedResult<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param, CancellationToken ct = default)
        {
            var result = await _userService.GetPagedAsync(param, ct);
            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,WarehouseAdmin,WarehouseEmployee")]
        [HttpGet("customers")]
        [EndpointName("GetCustomersPaged")]
        [EndpointSummary("Get paginated customers")]
        [EndpointDescription("Returns a paginated list of customers. Accessible by warehouse staff and admins.")]
        [ProducesResponseType(typeof(PagedResult<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCustomersPaged([FromQuery] PaginationParams param, CancellationToken ct = default)
        {
            var result = await _userService.GetCustomersAsync(param, ct);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [EndpointName("UpdateUser")]
        [EndpointSummary("Update user")]
        [EndpointDescription("Updates a user. Only the owner of the account can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO dto, CancellationToken ct = default)
        {
            var user = await _userService.GetByIdAsync(id, ct);

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                user,
                "UserOwnershipPolicy");

            if (!authResult.Succeeded)
                return Forbid();

            await _userService.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [EndpointName("DeleteUser")]
        [EndpointSummary("Delete user")]
        [EndpointDescription("Deletes a user. Only the owner of the account can perform this action.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var user = await _userService.GetByIdAsync(id, ct);

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                user,
                "UserOwnershipPolicy");

            if (!authResult.Succeeded)
                return Forbid();

            await _userService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}