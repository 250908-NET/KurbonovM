using Microsoft.AspNetCore.Mvc;
using BugTrakr.DTOs;
using BugTrakr.Services;

namespace YourProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var createdUser = await _authService.RegisterAsync(registerDto);
            if (createdUser == null)
            {
                // This indicates the username already exists
                return BadRequest("Username already exists.");
            }

            // Return a success message upon successful registration
            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
            if (token == null)
            {
                // Return 401 Unauthorized for invalid credentials
                return Unauthorized("Invalid username or password.");
            }

            // Return the JWT upon successful login
            return Ok(new { Token = token });
        }
    }
}