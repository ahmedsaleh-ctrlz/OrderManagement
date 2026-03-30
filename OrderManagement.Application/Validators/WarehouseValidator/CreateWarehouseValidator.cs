using FluentValidation;
using OrderManagement.Application.DTOs.WarehouseDTOs;

namespace OrderManagement.Application.Validators.WarehouseValidator
{
    public class CreateWarehouseValidator : AbstractValidator<CreateWarehouseDTO>
    {
        public CreateWarehouseValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
        }
    }
}
