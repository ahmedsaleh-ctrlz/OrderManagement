using OrderManagement.Application.Interfaces.Services;
using OrderManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.Application
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserServices, UserServices>();

            return services;
        }
    }
}
