using System.Reflection; 
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sprint.Helpers
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        private static readonly Dictionary<string, int> _httpMethodOrder = new()
        {
            {"GET", 1},
            {"POST", 2},
            {"PUT", 3},
            {"DELETE", 4}
        };

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
     
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateInfoForApiVersion(description));
            }

            options.TagActionsBy(api =>
            {
                var controllerName = api.ActionDescriptor.RouteValues["controller"];

                if (controllerName is string name)
                {
                    if (name.EndsWith("V"))
                    {
                        name = name.Substring(0, name.Length - 1);
                    }

                    if (name.EndsWith("Controller"))
                    {
                        name = name.Replace("Controller", "");
                    }
                    
                    var version = api.GetApiVersion().ToString();
                    
                    var finalName = $"{name} {version}";

                    return new[] { finalName };
                }
                
                return new[] { controllerName };
            });

            options.OrderActionsBy((apiDesc) => 
            {
                var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
                var version = apiDesc.GetApiVersion().ToString();
                
                var httpMethodOrder = _httpMethodOrder.GetValueOrDefault(apiDesc.HttpMethod?.ToUpperInvariant() ?? "", 99);
                
                return $"{version}_{httpMethodOrder}_{controllerName}";
            });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "MotoBlu - Sprint API",
                Version = description.ApiVersion.ToString(),
                Description = "Documentação da API do projeto Sprint MotoBlu."
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão da API está obsoleta.";
            }

            return info;
        }
    }
}