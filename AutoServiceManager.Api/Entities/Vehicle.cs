using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Entities;

public class Vehicle : AuditableEntity
{
    public int VehicleId { get; set; }

    public int CustomerId { get; set; }

    public string VIN { get; set; } = string.Empty;

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? PlateNumber { get; set; }

    public string? UnitNumber { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
}