using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Common.Validator
{
    public static class NameValidator
    {
        public static void Validate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            if (name.Length < 2)
                throw new ArgumentException("Name must be at least 2 characters long");
        }

    }
}
