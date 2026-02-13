using OrderManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderManagement.Application.Common.Validator
{
    public static class PasswordValidator
    {
        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new BadRequestException("Password is required");

            if (password.Length < 8)
                throw new BadRequestException("Password must be at least 8 characters");

            if (!Regex.IsMatch(password, "[A-Z]"))
                throw new BadRequestException("Password must contain at least one uppercase letter");

            if (!Regex.IsMatch(password, "[a-z]"))
                throw new BadRequestException("Password must contain at least one lowercase letter");

            if (!Regex.IsMatch(password, "[0-9]"))
                throw new BadRequestException("Password must contain at least one digit");

            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
                throw new BadRequestException("Password must contain at least one special character");
        }
    }
}
