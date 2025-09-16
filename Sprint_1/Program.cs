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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


await app.RunAsync();


