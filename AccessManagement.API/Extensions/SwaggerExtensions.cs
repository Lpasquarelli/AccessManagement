using AccessManagement.API.Configuration;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace AccessManagement.API.Extensions;

public static class SwaggerExtensions
{
  public static IServiceCollection AddSwagger(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    services.AddOptions<SwaggerSettings>()
      .Bind(configuration.GetSection(SwaggerSettings.SectionName))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
      })
      .AddMvc()
      .AddApiExplorer(options =>
      {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
      });

    var swaggerSettings = configuration
      .GetSection(SwaggerSettings.SectionName)
      .Get<SwaggerSettings>() ?? new SwaggerSettings();

    services.AddSwaggerGen(options =>
    {
      options.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = swaggerSettings.Title,
        Version = "v1",
        Description = swaggerSettings.Description
      });
    });

    return services;
  }

  public static WebApplication UseSwagger(this WebApplication app)
  {
    var settings = app.Services.GetRequiredService<IOptions<SwaggerSettings>>().Value;
    if (!settings.Enabled)
      return app;

    app.UseSwagger(_ => { });
    app.UseSwaggerUI(options =>
    {
      options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{settings.Title} v1");
      options.RoutePrefix = "swagger";
    });

    return app;
  }
}
