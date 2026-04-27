using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.Enums;

namespace AutoServiceManager.Api.Entities;

public class ServiceOrderOperation : AuditableEntity
{
    public int ServiceOrderOperationId { get; set; }

    public int ServiceOrderId { get; set; }

    public int? TechnicianId { get; set; }

    public string OpCode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal LaborHours { get; set; }

    public decimal LaborRate { get; set; }

    public decimal LaborAmount { get; set; }

    public OperationStatus Status { get; set; } = OperationStatus.Pending;

    public ServiceOrder? ServiceOrder { get; set; }

    public Technician? Technician { get; set; }
}