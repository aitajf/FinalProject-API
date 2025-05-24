using Domain.Entities;
using Service.DTO.Account;
using Service.Helpers.Account;

namespace Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResponse> RegisterAsync(RegisterDto model);
        Task<LoginResponse> LoginAsync(LoginDto model);
        Task CreateRoleAsync();
        Task<string> VerifyEmailAsync(string VerifyEmail, string token);
        string CreateToken(AppUser user, IList<string> roles);
    }
}
