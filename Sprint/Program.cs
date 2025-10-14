using Microsoft.EntityFrameworkCore;
using Sprint.Data;
using Oracle.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

// ---------- builder ----------
var builder = WebApplication.CreateBuilder(args);

// URL da aplicação
builder.WebHost.UseUrls("http://localhost:5051");

// Conexão com o banco de dados Oracle
var connString = builder.Configuration.GetConnectionString("OracleConnection");
if (string.IsNullOrEmpty(connString))
{
    throw new InvalidOperationException("A string de conexão 'OracleConnection' não foi encontrada.");
}

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("OracleDb", () =>
    {
        try
        {
            using var connection = new OracleConnection(connString);
            connection.Open();
            return HealthCheckResult.Healthy("Conexão com Oracle OK");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha ao conectar com Oracle", ex);
        }
    });

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connString));

// Registrar serviços
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IPatioService, PatioService>();

// Adicionar Controllers
builder.Services.AddControllers();

// Versionamento da API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});

// Versioned API Explorer
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Formato 'v1', 'v2'
    options.SubstituteApiVersionInUrl = true;
});

// --- SWAGGER ---
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>(); // Filtro de Operação

    // DocInclusionPredicate (FILTRO CORRIGIDO)
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

        // Procura atributos de ApiVersion no controller e métodos
        var versions = methodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(a => a.Versions)
            .Select(v => $"v{v.ToString("VVV")}") // CORREÇÃO: Usa o formato 'v1', 'v2' para o filtro
            .ToList() ?? new List<string>();

        // Também checa atributos no método
        var methodVersions = methodInfo
            .GetCustomAttributes(true)
            .OfType<MapToApiVersionAttribute>()
            .SelectMany(a => a.Versions)
            .Select(v => $"v{v.ToString("VVV")}"); // CORREÇÃO: Usa o formato 'v1', 'v2' para o filtro

        if (methodVersions.Any()) versions.AddRange(methodVersions);

        return versions.Contains(docName);
    });
});

// Configura geração por versão via IConfigureOptions (Classe ConfigureSwaggerOptions)
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Registrar o filtro de operações
builder.Services.AddTransient<SwaggerDefaultValues>();

// ---------- build ----------
var app = builder.Build();

// Exibe as versões registradas no console (útil para debugging)
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
foreach (var description in provider.ApiVersionDescriptions)
{
    Console.WriteLine($"Registered API version: {description.GroupName} (ApiVersion: {description.ApiVersion})");
}

// Middleware Swagger (UI + JSON)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Registra um endpoint de swagger.json por versão detectada
    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
    c.DocumentTitle = "MotoBlu - API Documentation";
});

// Middleware e rotas
app.UseHttpsRedirection();
app.MapControllers();

// Health Check com JSON detalhado
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        });
        await context.Response.WriteAsync(result);
    }
});

// Rodar aplicação
await app.RunAsync();