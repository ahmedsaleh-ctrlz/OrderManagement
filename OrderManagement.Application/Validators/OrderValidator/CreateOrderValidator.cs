using FluentValidation;
using OrderManagement.Application.DTOs.OrderDTOs;
using OrderManagement.Application.Validators.OrderItemValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Validators.OrderValidator
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDTO>
    { 
        public CreateOrderValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required , may need to login again.");
            RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("WarehouseId Is Requierd.");
            RuleForEach(x => x.Items).SetValidator(new CreateOrderItemValidator());
        }
    }
}
