using CarRentCalculator.Application.Services;
using CarRentCalculator.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IRentService, RentService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Car Rent Calculator",
            Description = "API's to calculate car rental prices.",
            Version = "v1",
            TermsOfService = null,
            Contact = new OpenApiContact()
        });
    var filePath = Path.Combine(AppContext.BaseDirectory, "CarRentCalculator.xml");
    o.IncludeXmlComments(filePath);
}); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
