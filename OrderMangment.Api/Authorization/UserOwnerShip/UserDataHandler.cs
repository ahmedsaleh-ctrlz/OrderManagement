using Microsoft.AspNetCore.Authorization;
using OrderManagement.Application.DTOs.UserDTOs;
using OrderManagement.Application.Interfaces.Global;
using OrderManagement.Application.Interfaces.Repositories;
using OrderManagement.Domain.Entites;
using OrderManagement.Domain.Enums;

namespace OrderManagementApi.Authorization.UserOwnerShip
{
    public class UserDataHandler : AuthorizationHandler<UserDataRequirement, UserDTO>
    {
        private readonly ICurrentUserService _currentUser;
        
        public UserDataHandler(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
           
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserDataRequirement requirement,
            UserDTO targetUser)
        {
           
            if (_currentUser.UserId == targetUser.Id)
            {
                context.Succeed(requirement);
                return;
            }

            
            if (!Enum.TryParse<UserRole>(_currentUser.Role, true, out var currentRole))
                return;
            if (!Enum.TryParse<UserRole>(targetUser.Role, true, out var targetUserRole))
                return;




            if (currentRole == UserRole.Customer)
                return;

           
            if (currentRole > targetUserRole)
            {
                context.Succeed(requirement);
            }
        }
    }
}