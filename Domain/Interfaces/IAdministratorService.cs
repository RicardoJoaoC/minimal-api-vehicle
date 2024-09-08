using MinimalApiVehicle.Domain.Entities;
using MinimalApiVehicle.DTOs;

namespace MinimalApiVehicle.Domain.Interfaces;

public interface IAdministratorService
{
    Administrator? Login(LoginDTO loginDTO);

}