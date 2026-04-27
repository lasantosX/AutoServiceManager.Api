using AutoServiceManager.Api.Data;
using Microsoft.EntityFrameworkCore;
using AutoServiceManager.Api.Services;
using AutoServiceManager.Api.Services.Interfaces;
using AutoServiceManager.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddScoped<ITechnicianService, TechnicianService>();

builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>();

builder.Services.AddScoped<IOperationService, OperationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoServiceManager.Api v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();