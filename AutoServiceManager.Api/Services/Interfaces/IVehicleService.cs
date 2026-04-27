using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.DTOs.Vehicles;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface IVehicleService
{
    Task<PagedResult<VehicleDto>> GetByCustomerIdAsync(int customerId, PagedRequest request);

    Task<VehicleDto?> GetByIdAsync(int id);

    Task<VehicleDto?> CreateAsync(int customerId, CreateVehicleRequest request);

    Task<bool> UpdateAsync(int id, UpdateVehicleRequest request);

    Task<bool> DeleteAsync(int id);
}