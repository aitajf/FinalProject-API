using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.DTO.Account;
using Service.Helpers;
using Service.Helpers.Account;
using Service.Helpers.Enums;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        private readonly SymmetricSecurityKey _securityKey;
        private readonly IConfiguration _configuration;
        public AccountService(UserManager<AppUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            SignInManager<AppUser> signInManager,
                            IHttpContextAccessor httpContextAccessor,
                            IEmailService emailService,
                            IMapper mapper,
                            IOptions<JwtSettings> jwtSettings,                          
                            IConfiguration configuration
                            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _mapper = mapper;          
            _jwtSettings = jwtSettings.Value;

            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]));
            _configuration = configuration;
        }

        public async Task CreateRoleAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole {Name = role.ToString()});
                }
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailOrUserName);

            if (user is null)
                user = await _userManager.FindByNameAsync(model.EmailOrUserName);

            if (user is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Login failed.",
                    Token = null
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Login failed.",
                    Token = null
                };
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Login failed: Email not confirmed.",
                    Token = null
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            string token = GenerateJwtToken(user.UserName, userRoles.ToList());

            return new LoginResponse
            {
                Success = true,
                Error = null,
                Token = token
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(m => m.Description)
                };
            }
            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());

            //Email confirm
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            string url = $"{baseUrl}/api/ui/Account/VerifyEmail?verifyEmail={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
            var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "confirm", "mailconfirm.html"));
            template = template.Replace("{{link}}", url);
            _emailService.Send(user.Email, "Email confirmation", template);

            return new RegisterResponse { Success = true, Errors = null };
        }

        public async Task<string> VerifyEmailAsync(string verifyEmail, string token)
        {
            var appUser = await _userManager.FindByEmailAsync(verifyEmail);
            if (appUser == null) return "User does not exist.";

            var result = await _userManager.ConfirmEmailAsync(appUser, token);
            if (!result.Succeeded) return string.Join(", ", result.Errors.Select(error => error.Description));

            await _userManager.UpdateSecurityStampAsync(appUser);
            var roles = await _userManager.GetRolesAsync(appUser);

            return CreateToken(appUser, roles);
        }


        public string CreateToken(AppUser user, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("FullName",user.FullName),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            SigningCredentials credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]

            };
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            var token = securityTokenHandler.CreateToken(tokenDescriptor);
            return securityTokenHandler.WriteToken(token);
        }



        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, username)
        };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpireDays));

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
