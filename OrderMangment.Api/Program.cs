using OrderManagement.Application;
using OrderManagement.Infrastructure;
using OrderManagementApi.Common.Extensions;
using OrderManagementApi;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApiServices();
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

app.MapControllers();

app.Run();


