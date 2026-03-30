using FluentValidation;
using OrderManagement.Application.DTOs.UserMangemanetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Validators.UserManagementValidator
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDTO>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(4).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
