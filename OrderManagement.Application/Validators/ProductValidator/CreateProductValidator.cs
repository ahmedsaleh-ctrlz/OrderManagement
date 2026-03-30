using FluentValidation;
using OrderManagement.Application.DTOs.ProductDTOs;


namespace OrderManagement.Application.Validators.ProductValidator
{
    public class CreateProductValidator : AbstractValidator<CreateProductDTO>
    {

        public CreateProductValidator() 
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(p => p.SKU)
                .NotEmpty().WithMessage("SKU is required.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(p => p.InitialQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Initial quantity must be greater than zero.");
        }

    }
}
