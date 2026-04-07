using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Threading.RateLimiting;

namespace OrderManagementApi.Common.Extensions
{
    public static class ApiServicesConfiguration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters
                        .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddRateLimiterService();

            return services;
        }



        private static IServiceCollection AddRateLimiterService(this IServiceCollection services) 
        {
            services.AddRateLimiter(options =>
            {
                    options.AddFixedWindowLimiter("DefaultPolicy", options =>
                    {
                        options.Window = TimeSpan.FromMinutes(1);
                        options.PermitLimit = 100;
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        options.QueueLimit = 10;
                    });

                    options.AddPolicy("IpPolicy", context =>
                    {
                        return RateLimitPartition.GetSlidingWindowLimiter(
                            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                            factory: _ => new SlidingWindowRateLimiterOptions
                            {
                                Window = TimeSpan.FromMinutes(1),
                                PermitLimit = 10,
                                SegmentsPerWindow = 6,
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                AutoReplenishment = true,
                            });
                            
                    });
            });

            return services;
        }

       
      
    }
}