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
ValidationError validationDTO(VehicleDTO vehicleDTO)
{
    var validation = new ValidationError{
        Messagens = new List<string>()
    };

    if(string.IsNullOrEmpty(vehicleDTO.Name))
        validation.Messagens.Add("The name can't be empty");

    if(string.IsNullOrEmpty(vehicleDTO.Make))
        validation.Messagens.Add("The make can't be empty");

    if(vehicleDTO.Ano < 1950)
        validation.Messagens.Add("Very old vehicle, only accepted after 1950");
        return validation;
}
app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) => {
    var validation = validationDTO(vehicleDTO);
    if(validation.Messagens.Count > 0)
        return Results.BadRequest(validation);

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
    
    var validation = validationDTO(vehicleDTO);
    if(validation.Messagens.Count > 0)
        return Results.BadRequest(validation);

    vehicle.Name = vehicleDTO.Name;
    vehicle.Make = vehicleDTO.Make;
    vehicle.Ano = vehicleDTO.Ano;

    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) => {
    
    var vehicle = vehicleService.FindById(id);
    if(vehicle == null) return Results.NotFound();

    vehicleService.Delete(vehicle);

    return Results.NoContent();
}).WithTags("Vehicles");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

#endregion