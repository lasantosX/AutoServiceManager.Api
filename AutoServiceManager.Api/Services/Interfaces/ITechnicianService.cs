using AutoServiceManager.Api.DTOs.Technicians;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface ITechnicianService
{
    Task<IEnumerable<TechnicianDto>> GetAllAsync();

    Task<TechnicianDto?> GetByIdAsync(int id);

    Task<TechnicianDto> CreateAsync(CreateTechnicianRequest request);

    Task<bool> UpdateAsync(int id, UpdateTechnicianRequest request);

    Task<bool> DeactivateAsync(int id);
}