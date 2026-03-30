using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Login endpoint - returns JWT token on success
        /// </summary>
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, token, user) = authService.Login(dto);

            if (!success)
                return Unauthorized(new { message = token });

            return Ok(new LoginResponse
            {
                Success = true,
                Token = token,
                User = user,
                Message = "Login successful"
            });
        }

        /// <summary>
        /// Register new user endpoint
        /// </summary>
        [HttpPost("register")]
        public ActionResult<RegisterResponse> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, user) = authService.Register(dto);

            if (!success)
                return BadRequest(new { message });

            return Created(string.Empty, new RegisterResponse
            {
                Success = true,
                User = user,
                Message = message
            });
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet("health")]
        public ActionResult<object> Health()
        {
            return Ok(new { status = "API is running" });
        }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public UserDTO User { get; set; }
        public string Message { get; set; }
    }

    public class RegisterResponse
    {
        public bool Success { get; set; }
        public UserDTO User { get; set; }
        public string Message { get; set; }
    }
}
