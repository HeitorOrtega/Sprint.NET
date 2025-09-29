using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Oracle.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("OracleConnection");

if (string.IsNullOrEmpty(connString))
{
    throw new InvalidOperationException("A string de conexão 'OracleConnection' não foi encontrada.");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connString));


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


await app.RunAsync();


