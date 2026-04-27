using AutoServiceManager.Api.Data;
using AutoServiceManager.Api.DTOs.ServiceOrders;
using AutoServiceManager.Api.Entities;
using AutoServiceManager.Api.Enums;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Services;

public class ServiceOrderService : IServiceOrderService
{
    private readonly AppDbContext _dbContext;

    public ServiceOrderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<ServiceOrderDto>> GetAllAsync(ServiceOrderPagedRequest request)
    {
        var query = _dbContext.ServiceOrders
            .AsNoTracking()
            .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(serviceOrder => serviceOrder.Status == request.Status.Value);
        }

        if (request.VehicleId.HasValue)
        {
            query = query.Where(serviceOrder => serviceOrder.VehicleId == request.VehicleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(serviceOrder =>
                serviceOrder.OrderNumber.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(serviceOrder => serviceOrder.OpenedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(serviceOrder => new ServiceOrderDto
            {
                ServiceOrderId = serviceOrder.ServiceOrderId,
                VehicleId = serviceOrder.VehicleId,
                OrderNumber = serviceOrder.OrderNumber,
                Status = serviceOrder.Status,
                OpenedAtUtc = serviceOrder.OpenedAtUtc,
                ClosedAtUtc = serviceOrder.ClosedAtUtc,
                TotalLaborAmount = serviceOrder.TotalLaborAmount,
                TotalPartsAmount = serviceOrder.TotalPartsAmount,
                TotalAmount = serviceOrder.TotalAmount
            })
            .ToListAsync();

        return new PagedResult<ServiceOrderDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ServiceOrderDto?> GetByIdAsync(int id)
    {
        return await _dbContext.ServiceOrders
            .AsNoTracking()
            .Where(serviceOrder => serviceOrder.ServiceOrderId == id)
            .Select(serviceOrder => new ServiceOrderDto
            {
                ServiceOrderId = serviceOrder.ServiceOrderId,
                VehicleId = serviceOrder.VehicleId,
                OrderNumber = serviceOrder.OrderNumber,
                Status = serviceOrder.Status,
                OpenedAtUtc = serviceOrder.OpenedAtUtc,
                ClosedAtUtc = serviceOrder.ClosedAtUtc,
                TotalLaborAmount = serviceOrder.TotalLaborAmount,
                TotalPartsAmount = serviceOrder.TotalPartsAmount,
                TotalAmount = serviceOrder.TotalAmount
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceOrderDto?> CreateAsync(CreateServiceOrderRequest request)
    {
        var vehicleExists = await _dbContext.Vehicles
            .AnyAsync(vehicle => vehicle.VehicleId == request.VehicleId);

        if (!vehicleExists)
        {
            return null;
        }

        var nextNumber = await GetNextOrderNumberAsync();

        var serviceOrder = new ServiceOrder
        {
            VehicleId = request.VehicleId,
            OrderNumber = nextNumber,
            Status = ServiceOrderStatus.Open,
            OpenedAtUtc = DateTime.UtcNow,
            TotalLaborAmount = 0,
            TotalPartsAmount = 0,
            TotalAmount = 0
        };

        _dbContext.ServiceOrders.Add(serviceOrder);
        await _dbContext.SaveChangesAsync();

        return new ServiceOrderDto
        {
            ServiceOrderId = serviceOrder.ServiceOrderId,
            VehicleId = serviceOrder.VehicleId,
            OrderNumber = serviceOrder.OrderNumber,
            Status = serviceOrder.Status,
            OpenedAtUtc = serviceOrder.OpenedAtUtc,
            ClosedAtUtc = serviceOrder.ClosedAtUtc,
            TotalLaborAmount = serviceOrder.TotalLaborAmount,
            TotalPartsAmount = serviceOrder.TotalPartsAmount,
            TotalAmount = serviceOrder.TotalAmount
        };
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateServiceOrderStatusRequest request)
    {
        var serviceOrder = await _dbContext.ServiceOrders
            .FirstOrDefaultAsync(serviceOrder => serviceOrder.ServiceOrderId == id);

        if (serviceOrder is null)
        {
            return false;
        }

        if (serviceOrder.Status == ServiceOrderStatus.Closed)
        {
            return false;
        }

        serviceOrder.Status = request.Status;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CloseAsync(int id)
    {
        var serviceOrder = await _dbContext.ServiceOrders
            .Include(serviceOrder => serviceOrder.Operations)
            .FirstOrDefaultAsync(serviceOrder => serviceOrder.ServiceOrderId == id);

        if (serviceOrder is null)
        {
            return false;
        }

        if (serviceOrder.Status == ServiceOrderStatus.Closed)
        {
            return true;
        }

        var hasOpenOperations = serviceOrder.Operations
            .Any(operation =>
                operation.Status != OperationStatus.Completed &&
                operation.Status != OperationStatus.Cancelled);

        if (hasOpenOperations)
        {
            return false;
        }

        serviceOrder.Status = ServiceOrderStatus.Closed;
        serviceOrder.ClosedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    private async Task<string> GetNextOrderNumberAsync()
    {
        var nextId = await _dbContext.ServiceOrders.CountAsync() + 1;

        return $"SO-{DateTime.UtcNow:yyyyMMdd}-{nextId:D5}";
    }
}