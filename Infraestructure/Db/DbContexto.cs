using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApiVehicle.Domain.Entities;

namespace MinimalApiVehicle.Infraestructure.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configurationAppSettings;
    public DbContexto(IConfiguration configurationAppSettings)
    {
        _configurationAppSettings = configurationAppSettings;
    }
    public DbSet<Administrator> Administrators { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>().HasData(
            new Administrator {
                Id = 1,
                Email = "administrator@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if( !optionsBuilder.IsConfigured)
        {
            var stringConection = _configurationAppSettings.GetConnectionString("mysql")?.ToString();
            if (!string.IsNullOrEmpty(stringConection))
            {
                optionsBuilder.UseMySql(
                    stringConection, 
                    ServerVersion.AutoDetect(stringConection)
            );}
        }
        
    }
}