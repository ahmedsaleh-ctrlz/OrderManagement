using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.ProductStockDTOs
{
    public class ProductQuantityDTO
    {
        [Range(1,int.MaxValue)]
        public int WarehouseId { get; set; }
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
    }
}
