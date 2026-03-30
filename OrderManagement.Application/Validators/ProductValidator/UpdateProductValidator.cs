using FluentValidation;
using OrderManagement.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Validators.ProductValidator
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.SKU)
                .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");

        }
    }
}
