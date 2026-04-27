using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.Enums;

namespace AutoServiceManager.Api.Entities;

public class ServiceOrder : AuditableEntity
{
    public int ServiceOrderId { get; set; }

    public int VehicleId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.Open;

    public DateTime OpenedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ClosedAtUtc { get; set; }

    public decimal TotalLaborAmount { get; set; }

    public decimal TotalPartsAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public Vehicle? Vehicle { get; set; }

    public ICollection<ServiceOrderOperation> Operations { get; set; } = new List<ServiceOrderOperation>();
}