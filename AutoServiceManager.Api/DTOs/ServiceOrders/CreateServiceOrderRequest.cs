using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Api.DTOs.ServiceOrders;

public class CreateServiceOrderRequest
{
    [Required]
    public int VehicleId { get; set; }
}