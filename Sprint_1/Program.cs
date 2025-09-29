using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("OracleConnection");
if (string.IsNullOrEmpty(connString))
{
    throw new InvalidOperationException("A string de conexão 'OracleConnection' não foi encontrada.");
}

// 1️⃣ Registrar DbContext antes de Build
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connString));

// 2️⃣ Registrar serviços
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IPatioService, PatioService>();

// 3️⃣ Registrar controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4️⃣ Build do app
var app = builder.Build();

// 5️⃣ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// 6️⃣ Rodar a aplicação
await app.RunAsync();