namespace MinimalApiVehicle.DTOs;
public record VehicleDTO
{
    public string Name { get; set; } = default!;
    public string Make { get; set; } = default!;
    public int Ano { get; set; } = default!;
}