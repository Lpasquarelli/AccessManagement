using AccessManagement.API.Configuration;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        Description = $"{swaggerSettings.Description}"
      });

      options.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);
      options.OperationFilter<EndpointMetadataOperationFilter>();
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

internal sealed class EndpointMetadataOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

    var summary = metadata
      .OfType<IEndpointSummaryMetadata>()
      .LastOrDefault()?.Summary;

    var description = metadata
      .OfType<IEndpointDescriptionMetadata>()
      .LastOrDefault()?.Description;

    if (!string.IsNullOrWhiteSpace(summary))
      operation.Summary = summary;

    if (!string.IsNullOrWhiteSpace(description))
      operation.Description = description;

    var standardResponses = new Dictionary<string, string>
    {
      ["400"] = "Payload ou regra de negócio inválida.",
      ["404"] = "Recurso inexistente ou pertencente a outra conta.",
      ["409"] = "Conflito de estado ou duplicidade.",
      ["422"] = "Configuração de alçada inválida.",
      ["503"] = "Dependência crítica indisponível; nenhuma autorização é concedida.",
      ["500"] = "Erro interno não detalhado ao consumidor."
    };

    operation.Responses ??= new OpenApiResponses();

    foreach (var response in standardResponses)
    {
      if (!operation.Responses.ContainsKey(response.Key))
      {
        operation.Responses.Add(response.Key, new OpenApiResponse
        {
          Description = response.Value
        });
      }
    }

    if (string.Equals(context.ApiDescription.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) &&
      !operation.Responses.ContainsKey("201"))
    {
      operation.Responses.Add("201", new OpenApiResponse
      {
        Description = "Recurso criado com sucesso."
      });
    }
  }
}
