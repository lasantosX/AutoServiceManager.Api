using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.DTOs.Customers;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> GetAllAsync(PagedRequest request);

    Task<CustomerDto?> GetByIdAsync(int id);

    Task<CustomerDto> CreateAsync(CreateCustomerRequest request);

    Task<bool> UpdateAsync(int id, UpdateCustomerRequest request);

    Task<bool> DeleteAsync(int id);
}