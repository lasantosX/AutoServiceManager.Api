using AutoServiceManager.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Api.DTOs.ServiceOrders;

public class UpdateServiceOrderStatusRequest
{
    [Required]
    public ServiceOrderStatus Status { get; set; }
}