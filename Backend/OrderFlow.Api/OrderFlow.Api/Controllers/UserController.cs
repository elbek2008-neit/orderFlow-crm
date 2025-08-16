using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
       private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var (users, total) = await _userService.GetUsersPagedAsync(pageNumber, pageSize, search);
            return Ok(new { total, users });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var updated = await _userService.UpdateUserAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/roles/{roleId}")]
        public async Task<IActionResult> AssignRole(int id, int roleId)
        {
            var result = await _userService.AssignRoleAsync(id, roleId);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole(int id, int roleId)
        {
            var result = await _userService.RemoveRoleAsync(id, roleId);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetRoles(int id)
        {
            var roles = await _userService.GetUserRolesAsync(id);
            return Ok(roles);
        }
    }
}
