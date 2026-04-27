using AutoServiceManager.Api.Data;
using AutoServiceManager.Api.DTOs.Customers;
using AutoServiceManager.Api.Entities;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _dbContext;

    public CustomerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<CustomerDto>> GetAllAsync(PagedRequest request)
    {
        var query = _dbContext.Customers
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(customer =>
                customer.FirstName.Contains(searchTerm) ||
                customer.LastName.Contains(searchTerm) ||
                (customer.Email != null && customer.Email.Contains(searchTerm)) ||
                (customer.Phone != null && customer.Phone.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(customer => customer.LastName)
            .ThenBy(customer => customer.FirstName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(customer => new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            })
            .ToListAsync();

        return new PagedResult<CustomerDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(customer => customer.CustomerId == id)
            .Select(customer => new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email?.Trim(),
            Phone = request.Phone?.Trim()
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.Phone
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(customer => customer.CustomerId == id);

        if (customer is null)
        {
            return false;
        }

        customer.FirstName = request.FirstName.Trim();
        customer.LastName = request.LastName.Trim();
        customer.Email = request.Email?.Trim();
        customer.Phone = request.Phone?.Trim();

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(customer => customer.CustomerId == id);

        if (customer is null)
        {
            return false;
        }

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}