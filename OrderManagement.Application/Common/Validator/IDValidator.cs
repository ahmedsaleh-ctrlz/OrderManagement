using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Common.Validator
{
    public static class IDValidator
    {
        public static void ValidateID(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be a positive integer.");
            }
        }
    }
}
