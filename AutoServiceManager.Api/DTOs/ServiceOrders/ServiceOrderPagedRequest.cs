using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.Enums;

namespace AutoServiceManager.Api.DTOs.ServiceOrders;

public class ServiceOrderPagedRequest : PagedRequest
{
    public ServiceOrderStatus? Status { get; set; }

    public int? VehicleId { get; set; }
}