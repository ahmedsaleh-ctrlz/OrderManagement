using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrderManagement.Application.DTOs.AuthDTOs;
using OrderManagement.Application.Services.Auth;

namespace OrderManagementApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("UserRegister")]
        [EndpointSummary("User register")]
        [EndpointDescription("Registers a new user with the provided details.")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto, CancellationToken ct = default)
        {
            var result = await _authService.RegisterAsync(dto, ct);
            return Ok(result);
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("UserLogin")]
        [EndpointSummary("User login")]
        [EndpointDescription("Authenticates a user and returns an authentication token.")]
        [EnableRateLimiting(policyName:"IpPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, CancellationToken ct = default)
        {
            var result = await _authService.LoginAsync(dto, ct);
            return Ok(result);
        }



        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("RefreshToken")]
        [EndpointSummary("Refresh authentication token")]
        [EndpointDescription("Refreshes the authentication token using a valid refresh token.")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request,CancellationToken ct)
        {
            return Ok(await _authService.RefreshTokenAsync(request,ct));
        }


        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [EndpointName("UserLogout")]
        [EndpointSummary("User logout")]
        [EndpointDescription("Logs out the user by invalidating the refresh token.")]
        public async Task<IActionResult> Logout(LogoutRequest request , CancellationToken ct)
        {
            await _authService.LogoutAsync(request, ct);
            return NoContent(); 

        }
    }
}
