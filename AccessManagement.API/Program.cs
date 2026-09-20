using AccessManagement.API.Contracts;
using AccessManagement.API.Exceptions;
using AccessManagement.API.Extensions;
using AccessManagement.Application.Di;
using AccessManagement.Infrastructure.Config;
using AccessManagement.Infrastructure.Di;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddControllers(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
  .ConfigureApiBehaviorOptions(options =>
  {
    options.InvalidModelStateResponseFactory = context =>
    {
      var errors = context.ModelState.Values
        .SelectMany(value => value.Errors)
        .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
          ? "The request body is invalid."
          : error.ErrorMessage)
        .ToArray();

      return new BadRequestObjectResult(new ErrorResponse(
        "The input is invalid.",
        errors.Length == 0 ? null : errors));
    };
  });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwagger(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseSwagger();
app.MapControllers();

var databaseOptions = app.Services
  .GetRequiredService<IOptions<DatabaseOptions>>()
  .Value;

if (app.Environment.IsDevelopment() && databaseOptions.ApplyMigrationsOnStartup)
  await app.Services.ApplyDatabaseMigrationsAsync();

await app.RunAsync();

public partial class Program;
