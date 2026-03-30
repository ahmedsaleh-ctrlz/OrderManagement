using FluentValidation;
using OrderManagement.Application.DTOs.WarehouseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Validators.WarehouseValidator
{
    public class UpdateWarehouseValidator : AbstractValidator<UpdateWarehouseDTO>
    {
        public UpdateWarehouseValidator() 
        {
            
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
        }
    }
}
