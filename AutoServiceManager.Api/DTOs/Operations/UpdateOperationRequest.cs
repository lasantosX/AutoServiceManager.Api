using AutoServiceManager.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Api.DTOs.Operations;

public class UpdateOperationRequest
{
    public int? TechnicianId { get; set; }

    [Required]
    [MaxLength(30)]
    public string OpCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 999.99)]
    public decimal LaborHours { get; set; }

    [Range(0, 9999.99)]
    public decimal LaborRate { get; set; }

    [Required]
    public OperationStatus Status { get; set; }
}