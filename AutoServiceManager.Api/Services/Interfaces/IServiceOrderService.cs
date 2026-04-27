using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.DTOs.ServiceOrders;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface IServiceOrderService
{
    Task<PagedResult<ServiceOrderDto>> GetAllAsync(ServiceOrderPagedRequest request);

    Task<ServiceOrderDto?> GetByIdAsync(int id);

    Task<ServiceOrderDto?> CreateAsync(CreateServiceOrderRequest request);

    Task<bool> UpdateStatusAsync(int id, UpdateServiceOrderStatusRequest request);

    Task<bool> CloseAsync(int id);
}