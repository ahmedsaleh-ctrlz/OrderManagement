using OrderManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.UserMangemanetDTOs
{
    public class EmployeesDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;

        private EmployeesDTO() { }

        public static EmployeesDTO FromModel(WarehouseUser warehouseUser)
        {
            return new EmployeesDTO
            {
                Id = warehouseUser.Id,
                FullName = warehouseUser.User.FullName,
                Email = warehouseUser.User.Email,
                WarehouseName = warehouseUser.Warehouse.Name
            };
        }

    }
}
