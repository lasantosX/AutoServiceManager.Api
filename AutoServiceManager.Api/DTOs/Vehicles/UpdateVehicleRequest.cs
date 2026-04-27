using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Api.DTOs.Vehicles;

public class UpdateVehicleRequest
{
    [Required]
    [MaxLength(17)]
    public string VIN { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int Year { get; set; }

    [MaxLength(20)]
    public string? PlateNumber { get; set; }

    [MaxLength(20)]
    public string? UnitNumber { get; set; }
}