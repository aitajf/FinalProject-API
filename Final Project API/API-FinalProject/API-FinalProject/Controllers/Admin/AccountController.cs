using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO.Account;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Admin
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoles()
        {
            await _accountService.CreateRoleAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _accountService.GetAllUsersAsync());
        }


        [HttpPost]
        public async Task<IActionResult> AddRole([FromQuery] RoleDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest("Invalid request.");
            }

            var response = await _accountService.AddRoleAsync(request.Username, request.RoleName);

            if (response.Contains("successfully", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole([FromQuery] RoleDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest("Invalid request.");
            }

            var response = await _accountService.RemoveRoleAsync(request.Username, request.RoleName);

            if (response.Contains("successfully", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }


        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _accountService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");

            var roles = await _accountService.GetUserRolesAsync(username);
            if (roles == null || !roles.Any())
                return NotFound("User not found or no roles assigned.");

            return Ok(roles);
        }
    }
}
