using FluentValidation;

namespace Service.DTO.Account
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("Username is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email format is wrong")
                                 .NotNull().WithMessage("Name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Password is required");
        }
    }
}
