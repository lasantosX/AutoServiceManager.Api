using AutoServiceManager.Api.Data;
using AutoServiceManager.Api.DTOs.Technicians;
using AutoServiceManager.Api.Entities;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceManager.Api.Services;

public class TechnicianService : ITechnicianService
{
    private readonly AppDbContext _dbContext;

    public TechnicianService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TechnicianDto>> GetAllAsync()
    {
        return await _dbContext.Technicians
            .AsNoTracking()
            .OrderBy(technician => technician.FullName)
            .Select(technician => new TechnicianDto
            {
                TechnicianId = technician.TechnicianId,
                FullName = technician.FullName,
                Email = technician.Email,
                IsActive = technician.IsActive
            })
            .ToListAsync();
    }

    public async Task<TechnicianDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Technicians
            .AsNoTracking()
            .Where(technician => technician.TechnicianId == id)
            .Select(technician => new TechnicianDto
            {
                TechnicianId = technician.TechnicianId,
                FullName = technician.FullName,
                Email = technician.Email,
                IsActive = technician.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TechnicianDto> CreateAsync(CreateTechnicianRequest request)
    {
        var technician = new Technician
        {
            FullName = request.FullName.Trim(),
            Email = request.Email?.Trim(),
            IsActive = true
        };

        _dbContext.Technicians.Add(technician);
        await _dbContext.SaveChangesAsync();

        return new TechnicianDto
        {
            TechnicianId = technician.TechnicianId,
            FullName = technician.FullName,
            Email = technician.Email,
            IsActive = technician.IsActive
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateTechnicianRequest request)
    {
        var technician = await _dbContext.Technicians
            .FirstOrDefaultAsync(technician => technician.TechnicianId == id);

        if (technician is null)
        {
            return false;
        }

        technician.FullName = request.FullName.Trim();
        technician.Email = request.Email?.Trim();
        technician.IsActive = request.IsActive;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var technician = await _dbContext.Technicians
            .FirstOrDefaultAsync(technician => technician.TechnicianId == id);

        if (technician is null)
        {
            return false;
        }

        technician.IsActive = false;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}