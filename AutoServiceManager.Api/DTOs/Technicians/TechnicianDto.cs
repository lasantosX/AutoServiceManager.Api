namespace AutoServiceManager.Api.DTOs.Technicians;

public class TechnicianDto
{
    public int TechnicianId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public bool IsActive { get; set; }
}