using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.Enums;

namespace AutoServiceManager.Api.DTOs.Operations;

public class OperationPagedRequest : PagedRequest
{
    public OperationStatus? Status { get; set; }

    public int? TechnicianId { get; set; }
}