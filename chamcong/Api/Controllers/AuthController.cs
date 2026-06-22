using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            var result = await _authService.GenerateTokenAsync(req);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result.Message);
            }
            return Ok(result.Data);
        }
    }
}
