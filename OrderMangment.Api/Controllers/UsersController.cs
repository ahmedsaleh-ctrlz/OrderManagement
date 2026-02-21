using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.DTOs.Paging;
using OrderManagement.Application.Services.Users;

namespace OrderManagementApi.Controllers
{
    /// <summary>
    /// Manages user operations.
    /// </summary>
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

        /// <summary>
        /// Retrieves user by ID.
        /// </summary>
        /// <remarks>
        /// Ownership rules apply:
        /// - User can access his own data.
        /// - Higher roles can access lower roles based on hierarchy.
        /// </remarks>
        /// <param name="id">Target user ID</param>
        /// <response code="200">User found</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                user,
                "UserOwnershipPolicy");

            if (!authResult.Succeeded)
                return Forbid();

            
            return Ok(user);
        }

        /// <summary>
        /// Retrieves paginated users.
        /// </summary>
        /// <remarks>
        /// Allowed roles:
        /// - SuperAdmin (all users)
        /// </remarks>
        /// <param name="param">Pagination parameters</param>
        /// <response code="200">Paged users returned</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams param)
        {
            var result = await _userService.GetPagedAsync(param);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves paginated users.
        /// </summary>
        /// <remarks>
        /// Allowed roles:
        /// - SuperAdmin 
        /// - WarehouseAdmin 
        /// - WarehouseEmployee 
        /// </remarks>
        /// <param name="param">Pagination parameters</param>
        /// <response code="200">Paged users returned</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "SuperAdmin,WarehouseAdmin,WarehouseEmployee")]
        [HttpGet("Customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCustomersPaged([FromQuery] PaginationParams param)
        {
            var result = await _userService.GetCustomersAsync(param);
            return Ok(result);
        }


        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <remarks>
        /// Ownership rules apply:
        /// - User can update his own account.
        /// - Higher roles can update lower roles.
        /// </remarks>
        /// <param name="id">Target user ID</param>
        /// <param name="dto">Updated user data</param>
        /// <response code="204">User updated successfully</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO dto)
        {
            var user = await _userService.GetByIdAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                user,
                "UserOwnershipPolicy");

            if (!authResult.Succeeded)
                return Forbid();

            await _userService.UpdateAsync(id, dto);
            return NoContent();
        }

        /// <summary>
        /// Soft deletes a user.
        /// </summary>
        /// <remarks>
        /// Ownership rules apply:
        /// - Only higher roles can delete lower roles.
        /// - User cannot delete himself unless business rules allow it.
        /// </remarks>
        /// <param name="id">Target user ID</param>
        /// <response code="204">User deleted successfully</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(
                User,
                user,
                "UserOwnershipPolicy");

            if (!authResult.Succeeded) 
            {
                return Forbid();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}