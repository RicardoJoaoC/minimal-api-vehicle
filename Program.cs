using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiVehicle.Domain.Interfaces;
using MinimalApiVehicle.Domain.Services;
using MinimalApiVehicle.DTOs;
using MinimalApiVehicle.Infraestructure.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    ));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) => {
    if (administratorService.Login(loginDTO)!= null)
        return Results.Ok("Login com Sucesso");
    else
        return Results.Unauthorized();  
});

app.Run();