using Microsoft.EntityFrameworkCore;
using SistemaBancoBack.Context;
using System;

var builder = WebApplication.CreateBuilder(args);

//variable de conexion a la base de datos
var connectionString = builder.Configuration.GetConnectionString("Connection");
//Registrar servicio para la conexion a la base de datos
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
