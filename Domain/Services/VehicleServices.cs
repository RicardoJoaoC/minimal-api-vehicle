using Microsoft.EntityFrameworkCore;
using MinimalApiVehicle.Domain.Entities;
using MinimalApiVehicle.Domain.Interfaces;
using MinimalApiVehicle.DTOs;
using MinimalApiVehicle.Infraestructure.Db;

namespace MinimalApiVehicle.Domain.Services;

public class VehicleService : IVehicleService
{
    private readonly DbContexto _contexto;

    public VehicleService(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public void Delete(Vehicle vehicle)
    {
        _contexto.vehicles.Remove(vehicle);
        _contexto.SaveChanges();
    }

    public Vehicle? FindById(int id)
    {
        return _contexto.vehicles.Where(v => v.Id == id).FirstOrDefault();
    }

    public void Include(Vehicle vehicle)
    {
        _contexto.vehicles.Add(vehicle);
        _contexto.SaveChanges();
    }

    public List<Vehicle> AllVehicles(int? page = 1, string? name = null, string? make = null)
    {
        var query = _contexto.vehicles.AsQueryable();
        if(!string.IsNullOrEmpty(name))
        {
            query = query.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name}%"));
        }

        int itensPerPage = 10;

        if (page != null){
            query = query.Skip(((int)page - 1) * itensPerPage).Take(itensPerPage);
        }

        return query.ToList();
    }

    public void Update(Vehicle vehicle)
    {
        _contexto.vehicles.Update(vehicle);
        _contexto.SaveChanges();
    }
}