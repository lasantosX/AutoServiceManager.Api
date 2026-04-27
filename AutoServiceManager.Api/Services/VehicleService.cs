using AutoServiceManager.Api.Data;
using AutoServiceManager.Api.DTOs.Vehicles;
using AutoServiceManager.Api.Entities;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Services;

public class VehicleService : IVehicleService
{
    private readonly AppDbContext _dbContext;

    public VehicleService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<VehicleDto>> GetByCustomerIdAsync(int customerId, PagedRequest request)
    {
        var query = _dbContext.Vehicles
            .AsNoTracking()
            .Where(vehicle => vehicle.CustomerId == customerId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(vehicle =>
                vehicle.VIN.Contains(searchTerm) ||
                vehicle.Make.Contains(searchTerm) ||
                vehicle.Model.Contains(searchTerm) ||
                (vehicle.PlateNumber != null && vehicle.PlateNumber.Contains(searchTerm)) ||
                (vehicle.UnitNumber != null && vehicle.UnitNumber.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(vehicle => vehicle.Make)
            .ThenBy(vehicle => vehicle.Model)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(vehicle => new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                CustomerId = vehicle.CustomerId,
                VIN = vehicle.VIN,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                PlateNumber = vehicle.PlateNumber,
                UnitNumber = vehicle.UnitNumber
            })
            .ToListAsync();

        return new PagedResult<VehicleDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<VehicleDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Vehicles
            .AsNoTracking()
            .Where(vehicle => vehicle.VehicleId == id)
            .Select(vehicle => new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                CustomerId = vehicle.CustomerId,
                VIN = vehicle.VIN,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                PlateNumber = vehicle.PlateNumber,
                UnitNumber = vehicle.UnitNumber
            })
            .FirstOrDefaultAsync();
    }

    public async Task<VehicleDto?> CreateAsync(int customerId, CreateVehicleRequest request)
    {
        var customerExists = await _dbContext.Customers
            .AnyAsync(customer => customer.CustomerId == customerId);

        if (!customerExists)
        {
            return null;
        }

        var vehicle = new Vehicle
        {
            CustomerId = customerId,
            VIN = request.VIN.Trim(),
            Make = request.Make.Trim(),
            Model = request.Model.Trim(),
            Year = request.Year,
            PlateNumber = request.PlateNumber?.Trim(),
            UnitNumber = request.UnitNumber?.Trim()
        };

        _dbContext.Vehicles.Add(vehicle);
        await _dbContext.SaveChangesAsync();

        return new VehicleDto
        {
            VehicleId = vehicle.VehicleId,
            CustomerId = vehicle.CustomerId,
            VIN = vehicle.VIN,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            PlateNumber = vehicle.PlateNumber,
            UnitNumber = vehicle.UnitNumber
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateVehicleRequest request)
    {
        var vehicle = await _dbContext.Vehicles
            .FirstOrDefaultAsync(vehicle => vehicle.VehicleId == id);

        if (vehicle is null)
        {
            return false;
        }

        vehicle.VIN = request.VIN.Trim();
        vehicle.Make = request.Make.Trim();
        vehicle.Model = request.Model.Trim();
        vehicle.Year = request.Year;
        vehicle.PlateNumber = request.PlateNumber?.Trim();
        vehicle.UnitNumber = request.UnitNumber?.Trim();

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _dbContext.Vehicles
            .FirstOrDefaultAsync(vehicle => vehicle.VehicleId == id);

        if (vehicle is null)
        {
            return false;
        }

        _dbContext.Vehicles.Remove(vehicle);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}