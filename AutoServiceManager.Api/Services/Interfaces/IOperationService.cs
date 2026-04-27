using AutoServiceManager.Api.DTOs.Operations;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface IOperationService
{
    Task<IEnumerable<OperationDto>> GetByServiceOrderIdAsync(int serviceOrderId);

    Task<OperationDto?> GetByIdAsync(int id);

    Task<OperationDto?> CreateAsync(int serviceOrderId, CreateOperationRequest request);

    Task<bool> UpdateAsync(int id, UpdateOperationRequest request);

    Task<bool> DeleteAsync(int id);
}