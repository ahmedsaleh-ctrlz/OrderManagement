using FluentValidation;
using OrderManagement.Application.DTOs.AuthDTOs;


namespace OrderManagement.Application.Validators.AuthValidator
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator() 
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(4).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
