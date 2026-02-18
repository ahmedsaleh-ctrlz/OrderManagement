using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Services.Auth;
using OrderManagement.Application.Services.Users;
using OrderManagement.Application.Services.Orders;
using OrderManagement.Application.Services.Products;
using OrderManagement.Application.Services.ProductStocks;
using OrderManagement.Application.Services.UsersManagement;
using OrderManagement.Application.Services.Warhouses;
using OrderManagement.Application.Services.WarhouseUsers;

namespace OrderManagement.Application
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IWarehouseServices, WarehouseServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IProductStockServices, ProductStockServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IWarehouseUserService, WarehouseUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserManagementService, UserManagementService>();

            return services;
        }
    }
}
