using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.Services.Auth;

namespace OrderManagementApi.Controllers
{
    /// <summary>
    /// Handles authentication operations such as registration and login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with default role "Customer".
        /// </summary>
        /// <remarks>
        /// - Email must be unique.
        /// - Password must meet security requirements.
        /// - Role is automatically assigned as Customer.
        /// </remarks>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Validation failed or email already exists</response>
        /// <response code="500">Unexpected server error</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Authenticates a user and returns JWT token.
        /// </summary>
        /// <remarks>
        /// - Returns access token containing user id and role claims.
        /// - Token expiration is configured in application settings.
        /// </remarks>
        /// <response code="200">Login successful</response>
        /// <response code="401">Invalid credentials</response>
        /// <response code="500">Unexpected server error</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
    }
}
