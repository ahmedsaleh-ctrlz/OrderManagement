using FluentValidation;
using OrderManagement.Application.DTOs.OrderItemDTOs;


namespace OrderManagement.Application.Validators.OrderItemValidator
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDTO>
    {
        public CreateOrderItemValidator() 
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        }
    }
}
