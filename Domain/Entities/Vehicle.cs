using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApiVehicle.Domain.Entities;

public class Vehicle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = default!;
    [Required]
    [StringLength(100)]
    public string Make { get; set; } = default!;
    [Required]
    [StringLength(10)]
    public int Ano { get; set; } = default!;
}