using Microsoft.EntityFrameworkCore;
using MinimalApiVehicle.Domain.Entities;
using MinimalApiVehicle.Domain.Interfaces;
using MinimalApiVehicle.DTOs;
using MinimalApiVehicle.Infraestructure.Db;

namespace MinimalApiVehicle.Domain.Services;

public class AdministratorService : IAdministratorService
{
    private readonly DbContexto _contexto;

    public AdministratorService(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Administrator? Login(LoginDTO loginDTO)
    {
        var adm = _contexto.Administrators.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }
}