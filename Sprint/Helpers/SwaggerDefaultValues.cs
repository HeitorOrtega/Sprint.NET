using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Parameters ??= new List<OpenApiParameter>();

        // Adiciona o header x-api-version se não existir
        if (!operation.Parameters.Any(p => p.Name == "x-api-version"))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-api-version",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Versão da API",
            });
        }
    }
}