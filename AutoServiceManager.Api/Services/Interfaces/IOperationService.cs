using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.DTOs.Operations;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface IOperationService
{
    Task<PagedResult<OperationDto>> GetByServiceOrderIdAsync(
        int serviceOrderId,
        OperationPagedRequest request);

    Task<OperationDto?> GetByIdAsync(int id);

    Task<OperationDto?> CreateAsync(int serviceOrderId, CreateOperationRequest request);

    Task<bool> UpdateAsync(int id, UpdateOperationRequest request);

    Task<bool> DeleteAsync(int id);
}