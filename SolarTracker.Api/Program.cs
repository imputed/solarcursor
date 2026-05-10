using FluentValidation;
using Scalar.AspNetCore;
using SolarTracker.Api.Endpoints;
using SolarTracker.Api.Exceptions;
using SolarTracker.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddValidatorsFromAssembly(typeof(ApiEndpointRegistration).Assembly);

builder.Services.AddOpenApi();
builder.Services.AddSolarInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapSolarTrackerApiEndpoints();

app.Run();
