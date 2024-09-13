using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiVehicle.Domain.Entities;
using MinimalApiVehicle.Domain.Interfaces;
using MinimalApiVehicle.Domain.ModelViews;
using MinimalApiVehicle.Domain.Services;
using MinimalApiVehicle.DTOs;
using MinimalApiVehicle.Infraestructure.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    ));

var app = builder.Build();

#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administrators
app.MapPost("/administrators/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) => {
    if (administratorService.Login(loginDTO)!= null)
        return Results.Ok("Login com Sucesso");
    else
        return Results.Unauthorized();  
}).WithTags("Administrators");
#endregion

#region Vehicles
app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) => {
    var vehicle = new Vehicle{
        Name = vehicleDTO.Name,
        Make = vehicleDTO.Make,
        Ano = vehicleDTO.Ano
    };
    vehicleService.Include(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);

}).WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) => {
    var vehicle = vehicleService.AllVehicles(page);

    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapGet("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) => {
    
    var vehicle = vehicleService.FindById(id);
    if (vehicle == null) return Results.NotFound();

    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapPut("/vehicles/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) => {
    
    var vehicle = vehicleService.FindById(id);
    if (vehicle == null) return Results.NotFound();

    vehicle.Name = vehicleDTO.Name;
    vehicle.Make = vehicleDTO.Make;
    vehicle.Ano = vehicleDTO.Ano;

    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);
}).WithTags("Vehicles");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

#endregion