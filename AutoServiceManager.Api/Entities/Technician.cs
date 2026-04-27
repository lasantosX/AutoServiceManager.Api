using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Entities;

public class Technician : AuditableEntity
{
    public int TechnicianId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<ServiceOrderOperation> Operations { get; set; } = new List<ServiceOrderOperation>();
}