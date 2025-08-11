using CrmOrderManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CrmOrderManagement.Core.AuthEntities;
using CrmOrderManagement.Core.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OrderFlow.Api.Controllers
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
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            return Ok(await _authService.LoginAsync(dto));
        }

        [HttpPost("register")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            return Ok(await _authService.RegisterAsync(dto));
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest dto)
        {
            return Ok(await _authService.RefreshTokenAsync(dto));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userDto = await _authService.GetCurrentUserAsync(userId);
            return Ok(userDto);
        }
    }
}
