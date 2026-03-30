using FluentValidation;
using OrderManagement.Application.DTOs.AuthDTOs;


namespace OrderManagement.Application.Validators.AuthValidator
{
    public class LoginValidator :AbstractValidator<LoginDTO>
    {
        public LoginValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                                  .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                                     .MinimumLength(4).WithMessage("Password must be at least 6 characters long");
        }
    }
}
