using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClaimsDesk.API.SwaggerExtensions;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"ClaimsDesk API v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = description.IsDeprecated
                    ? "This API version is deprecated."
                    : "Core claims engine and workflow endpoints."
            });
        }

        // Add App ID Security Definition
        options.AddSecurityDefinition("AppId", new OpenApiSecurityScheme
        {
            Description = "App ID authentication. Enter your App ID below.",
            In = ParameterLocation.Header,
            Name = "X-App-Id",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "AppIdScheme"
        });

        // Add API Key Security Definition
        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "API Key authentication. Enter your key below.",
            In = ParameterLocation.Header,
            Name = "X-Api-Key",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "ApiKeyScheme"
        });

        options.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.OpenApiSecuritySchemeReference("AppId", doc),
                new System.Collections.Generic.List<string>()
            },
            {
                new Microsoft.OpenApi.OpenApiSecuritySchemeReference("ApiKey", doc),
                new System.Collections.Generic.List<string>()
            }
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
        if (System.IO.File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    }
}

