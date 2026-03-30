using FluentValidation;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;


namespace OrderManagement.Application.Validators.UserManagementValidator
{
    public class CreateAdminValidator : AbstractValidator<CreateAdminDTO>
    {
        public CreateAdminValidator() 
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(4).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0).WithMessage("Warehouse ID must be greater than 0.");
        }
    }
}
