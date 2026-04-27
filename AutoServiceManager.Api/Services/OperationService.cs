using AutoServiceManager.Api.Data;
using AutoServiceManager.Api.DTOs.Operations;
using AutoServiceManager.Api.Entities;
using AutoServiceManager.Api.Enums;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceManager.Api.Services;

public class OperationService : IOperationService
{
    private readonly AppDbContext _dbContext;

    public OperationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<OperationDto>> GetByServiceOrderIdAsync(int serviceOrderId)
    {
        return await _dbContext.ServiceOrderOperations
            .AsNoTracking()
            .Where(operation => operation.ServiceOrderId == serviceOrderId)
            .OrderBy(operation => operation.OpCode)
            .Select(operation => new OperationDto
            {
                ServiceOrderOperationId = operation.ServiceOrderOperationId,
                ServiceOrderId = operation.ServiceOrderId,
                TechnicianId = operation.TechnicianId,
                OpCode = operation.OpCode,
                Description = operation.Description,
                LaborHours = operation.LaborHours,
                LaborRate = operation.LaborRate,
                LaborAmount = operation.LaborAmount,
                Status = operation.Status
            })
            .ToListAsync();
    }

    public async Task<OperationDto?> GetByIdAsync(int id)
    {
        return await _dbContext.ServiceOrderOperations
            .AsNoTracking()
            .Where(operation => operation.ServiceOrderOperationId == id)
            .Select(operation => new OperationDto
            {
                ServiceOrderOperationId = operation.ServiceOrderOperationId,
                ServiceOrderId = operation.ServiceOrderId,
                TechnicianId = operation.TechnicianId,
                OpCode = operation.OpCode,
                Description = operation.Description,
                LaborHours = operation.LaborHours,
                LaborRate = operation.LaborRate,
                LaborAmount = operation.LaborAmount,
                Status = operation.Status
            })
            .FirstOrDefaultAsync();
    }

    public async Task<OperationDto?> CreateAsync(int serviceOrderId, CreateOperationRequest request)
    {
        var serviceOrder = await _dbContext.ServiceOrders
            .Include(order => order.Operations)
            .FirstOrDefaultAsync(order => order.ServiceOrderId == serviceOrderId);

        if (serviceOrder is null || serviceOrder.Status == ServiceOrderStatus.Closed)
        {
            return null;
        }

        if (request.TechnicianId.HasValue)
        {
            var technicianExists = await _dbContext.Technicians
                .AnyAsync(technician =>
                    technician.TechnicianId == request.TechnicianId.Value &&
                    technician.IsActive);

            if (!technicianExists)
            {
                return null;
            }
        }

        var operation = new ServiceOrderOperation
        {
            ServiceOrderId = serviceOrderId,
            TechnicianId = request.TechnicianId,
            OpCode = request.OpCode.Trim(),
            Description = request.Description.Trim(),
            LaborHours = request.LaborHours,
            LaborRate = request.LaborRate,
            LaborAmount = CalculateLaborAmount(request.LaborHours, request.LaborRate),
            Status = OperationStatus.Pending
        };

        _dbContext.ServiceOrderOperations.Add(operation);

        serviceOrder.Status = ServiceOrderStatus.InProgress;

        await _dbContext.SaveChangesAsync();

        await RecalculateServiceOrderTotalsAsync(serviceOrderId);

        return new OperationDto
        {
            ServiceOrderOperationId = operation.ServiceOrderOperationId,
            ServiceOrderId = operation.ServiceOrderId,
            TechnicianId = operation.TechnicianId,
            OpCode = operation.OpCode,
            Description = operation.Description,
            LaborHours = operation.LaborHours,
            LaborRate = operation.LaborRate,
            LaborAmount = operation.LaborAmount,
            Status = operation.Status
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateOperationRequest request)
    {
        var operation = await _dbContext.ServiceOrderOperations
            .Include(operation => operation.ServiceOrder)
            .FirstOrDefaultAsync(operation => operation.ServiceOrderOperationId == id);

        if (operation is null || operation.ServiceOrder is null)
        {
            return false;
        }

        if (operation.ServiceOrder.Status == ServiceOrderStatus.Closed)
        {
            return false;
        }

        if (request.TechnicianId.HasValue)
        {
            var technicianExists = await _dbContext.Technicians
                .AnyAsync(technician =>
                    technician.TechnicianId == request.TechnicianId.Value &&
                    technician.IsActive);

            if (!technicianExists)
            {
                return false;
            }
        }

        operation.TechnicianId = request.TechnicianId;
        operation.OpCode = request.OpCode.Trim();
        operation.Description = request.Description.Trim();
        operation.LaborHours = request.LaborHours;
        operation.LaborRate = request.LaborRate;
        operation.LaborAmount = CalculateLaborAmount(request.LaborHours, request.LaborRate);
        operation.Status = request.Status;

        await _dbContext.SaveChangesAsync();

        await RecalculateServiceOrderTotalsAsync(operation.ServiceOrderId);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var operation = await _dbContext.ServiceOrderOperations
            .Include(operation => operation.ServiceOrder)
            .FirstOrDefaultAsync(operation => operation.ServiceOrderOperationId == id);

        if (operation is null || operation.ServiceOrder is null)
        {
            return false;
        }

        if (operation.ServiceOrder.Status == ServiceOrderStatus.Closed)
        {
            return false;
        }

        var serviceOrderId = operation.ServiceOrderId;

        _dbContext.ServiceOrderOperations.Remove(operation);
        await _dbContext.SaveChangesAsync();

        await RecalculateServiceOrderTotalsAsync(serviceOrderId);

        return true;
    }

    private static decimal CalculateLaborAmount(decimal laborHours, decimal laborRate)
    {
        return Math.Round(laborHours * laborRate, 2);
    }

    private async Task RecalculateServiceOrderTotalsAsync(int serviceOrderId)
    {
        var serviceOrder = await _dbContext.ServiceOrders
            .Include(order => order.Operations)
            .FirstOrDefaultAsync(order => order.ServiceOrderId == serviceOrderId);

        if (serviceOrder is null)
        {
            return;
        }

        serviceOrder.TotalLaborAmount = serviceOrder.Operations.Sum(operation => operation.LaborAmount);
        serviceOrder.TotalAmount = serviceOrder.TotalLaborAmount + serviceOrder.TotalPartsAmount;

        await _dbContext.SaveChangesAsync();
    }
}