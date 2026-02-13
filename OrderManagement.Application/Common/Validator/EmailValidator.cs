using OrderManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Common.Validator
{
    public static class EmailValidator
    {
        public static void Validate(string email)
        {
       
            if (string.IsNullOrWhiteSpace(email))
                throw new BadRequestException("Email is required");

            if (!email.Contains("@"))
                throw new BadRequestException("Invalid email format");
        }
    }
}
