using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Api.DTOs.Technicians;

public class UpdateTechnicianRequest
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(150)]
    public string? Email { get; set; }

    public bool IsActive { get; set; }
}