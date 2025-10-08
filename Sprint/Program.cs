using Microsoft.EntityFrameworkCore;
using Sprint.Data;
using Oracle.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Conexão com o banco de dados Oracle
var connString = builder.Configuration.GetConnectionString("OracleConnection");
if (string.IsNullOrEmpty(connString))
{
    throw new InvalidOperationException("A string de conexão 'OracleConnection' não foi encontrada.");
}

// 2️⃣ Registrar o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connString));

// 3️⃣ Registrar serviços
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IPatioService, PatioService>();

// 4️⃣ Adicionar Controllers e Swagger
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API MotoBlu - Sprint 4",
        Version = "v1",
        Description = "API RESTful desenvolvida em ASP.NET Core, com integração ao Oracle Database. Segue boas práticas de design HTTP (status codes, verbos e estrutura de resposta) e arquitetura em camadas (Controller, Service, Repository).",
        Contact = new OpenApiContact
        {
            Name = "Equipe MotoBlu",
            Email = "contato@motoblu.com.br",
            Url = new Uri("https://github.com/MotoBlu")
        },
        License = new OpenApiLicense
        {
            Name = "Uso acadêmico - FIAP"
        }
    });

    // Inclui comentários XML se existirem (gera descrições automáticas dos endpoints)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// 5️⃣ Build da aplicação
var app = builder.Build();

// 6️⃣ Middleware e Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API MotoBlu v1");
        c.DocumentTitle = "MotoBlu - API Documentation";
    });
}

// 7️⃣ HTTPS e rotas
app.UseHttpsRedirection();
app.MapControllers();

// 8️⃣ Rodar aplicação
await app.RunAsync();
