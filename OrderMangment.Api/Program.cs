using OrderManagement.Application;
using OrderManagement.Infrastructure;
using OrderManagementApi.Common.Extensions;
using OrderManagementApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.


builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApiServices();

builder.Host.UseSerilog((Context, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddCustomAuthorization();


var app = builder.Build();


app.UseStatusCodePages();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();
app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();


