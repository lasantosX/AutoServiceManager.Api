using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Entities;

public class Customer : AuditableEntity
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}