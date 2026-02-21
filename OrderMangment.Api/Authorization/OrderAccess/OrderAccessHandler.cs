using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using System.Security.Claims;

namespace OrderManagementApi.Authorization.OrderAccess
{
    public class OrderAccessHandler : AuthorizationHandler<OrderAccessRequiremnt, int>
    {
        private readonly IOrderRepository _orderRepo;

        public OrderAccessHandler(IOrderRepository orderRepository)
        {
            _orderRepo = orderRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderAccessRequiremnt requirement, int orderId)
        {
            var order = await _orderRepo.GetWithDetailsAsync(orderId);
            if (order is null)
                return;



            //Super Admin Check
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return;
            }



            //Customer Check
            if (context.User.IsInRole("Customer"))
            {
                var AuthUserId = Convert.ToInt32(context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (order.UserId == AuthUserId)
                {
                    context.Succeed(requirement);
                    return;
                }

            }

            if (context.User.IsInRole("WarehouseAdmin") ||
                context.User.IsInRole("WarehouseEmployee"))
            {
                var warehouseIdClaim = context.User.FindFirst("WarehouseId");
                if(warehouseIdClaim != null)
                {
                    var warehouseId = Convert.ToInt32(warehouseIdClaim.Value);
                    if(order.WarehouseId ==warehouseId) 
                    {
                        context.Succeed(requirement);
                        return;
                    }   
                }
            }

        }
    }
}
