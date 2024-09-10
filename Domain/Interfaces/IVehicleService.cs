using MinimalApiVehicle.Domain.Entities;
using MinimalApiVehicle.DTOs;

namespace MinimalApiVehicle.Domain.Interfaces;

public interface IVehicleService
{
    List<Vehicle> AllVehicles(int? page = 1, string? name = null, string? make = null);
    Vehicle? FindById(int id);
    void Include(Vehicle vehicle);
    void Update(Vehicle vehicle);
    void Delete(Vehicle vehicle);
}