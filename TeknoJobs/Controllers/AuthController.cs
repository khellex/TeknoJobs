using Microsoft.AspNetCore.Mvc;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Application.Interfaces;

namespace TeknoJobs.Controllers
{
    /// <summary>
    /// Provides authentication-related endpoints for managing user login and token generation.
    /// </summary>
    /// <remarks>This controller handles user authentication requests, including login functionality.  It
    /// generates JSON Web Tokens (JWTs) for authenticated users, which can be used for  subsequent API requests
    /// requiring authorization. The controller is part of the API version 1 group.</remarks>
    [ApiController]
    [Route("api/v1/auth")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Handles user login and returns a token (JWT) if successful.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>Returns a token for successful login or Unauthorized for failed login.</returns>
        /// <response code="200">Returns the token</response>
        /// <response code="400">If either or both the username and password are left empty.</response>
        /// <response code="401">Returns Unauthorized if either or both username and password are wrong.</response>
        /// <response code="500">If any unexpected error occurs.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Username and password are required.");
                }
                //we're hard coding the credentials for convenience in this example, on production 
                //we would be using a real authentication service ( IAuthService ) to validate credentials
                if (request.Username == "admin" && request.Password == "password")
                {
                    var token = _tokenService.GenerateToken(userId: "1", username: "admin", role: "Admin");
                    return Ok(new { token });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
