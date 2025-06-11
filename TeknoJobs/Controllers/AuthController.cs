using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Application.Interfaces;

namespace TeknoJobs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Handles user login and returns a token if successful.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>Returns a token for successful login or Unauthorized for failed login.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }
            if (request.Username == "admin" && request.Password == "password")
            {
                // Need to implement the IAuthService to handle real authentication logic
                var token = _tokenService.GenerateToken("1", "admin", "Admin");
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
