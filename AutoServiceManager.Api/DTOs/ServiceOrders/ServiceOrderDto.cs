using AutoServiceManager.Api.Enums;

namespace AutoServiceManager.Api.DTOs.ServiceOrders;

public class ServiceOrderDto
{
    public int ServiceOrderId { get; set; }

    public int VehicleId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public ServiceOrderStatus Status { get; set; }

    public DateTime OpenedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }

    public decimal TotalLaborAmount { get; set; }

    public decimal TotalPartsAmount { get; set; }

    public decimal TotalAmount { get; set; }
}