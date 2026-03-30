using FluentValidation;
using OrderManagement.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Validators.UserValidator
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserValidator() 
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");
        }
    }
}
