using Microsoft.AspNetCore.Authorization;
using OrderManagementApi.Authorization.UserOwnerShip;
using OrderManagementApi.Authorization.OrderAccess;

namespace OrderManagementApi
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserOwnershipPolicy", policy =>
                    policy.Requirements.Add(new UserDataRequirement()));

                options.AddPolicy("OrderAccessPolicy", policy =>
                    policy.Requirements.Add(new OrderAccessRequiremnt()));
            });

            services.AddScoped<IAuthorizationHandler, UserDataHandler>();
            services.AddScoped<IAuthorizationHandler, OrderAccessHandler>();

            return services;
        }
    }
}