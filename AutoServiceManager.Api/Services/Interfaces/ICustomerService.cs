using AutoServiceManager.Api.DTOs.Customers;

namespace AutoServiceManager.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<CustomerDto?> GetByIdAsync(int id);

    Task<CustomerDto> CreateAsync(CreateCustomerRequest request);

    Task<bool> UpdateAsync(int id, UpdateCustomerRequest request);

    Task<bool> DeleteAsync(int id);
}