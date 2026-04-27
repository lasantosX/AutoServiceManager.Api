using AutoServiceManager.Api.Enums;

namespace AutoServiceManager.Api.DTOs.Operations;

public class OperationDto
{
    public int ServiceOrderOperationId { get; set; }

    public int ServiceOrderId { get; set; }

    public int? TechnicianId { get; set; }

    public string OpCode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal LaborHours { get; set; }

    public decimal LaborRate { get; set; }

    public decimal LaborAmount { get; set; }

    public OperationStatus Status { get; set; }
}