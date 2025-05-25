using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO.Account;
using Service.Services;
using Service.Services.Interfaces;

namespace API_FinalProject.Controllers.Client
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _accountService.RegisterAsync(request);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            return Ok(await _accountService.LoginAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string verifyEmail, string token)
        {
            if (VerifyEmail == null || token == null) return BadRequest("Something went wrong");
            var response = await _accountService.VerifyEmail(verifyEmail, token);
            //return Redirect("https://localhost:7004/api/Account/Login");
            return Ok(response);
        }
    }
}
