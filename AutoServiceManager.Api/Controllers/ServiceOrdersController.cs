using AutoServiceManager.Api.DTOs.ServiceOrders;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderService _serviceOrderService;

    public ServiceOrdersController(IServiceOrderService serviceOrderService)
    {
        _serviceOrderService = serviceOrderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> GetAll()
    {
        var serviceOrders = await _serviceOrderService.GetAllAsync();

        return Ok(serviceOrders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceOrderDto>> GetById(int id)
    {
        var serviceOrder = await _serviceOrderService.GetByIdAsync(id);

        if (serviceOrder is null)
        {
            return NotFound();
        }

        return Ok(serviceOrder);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceOrderDto>> Create(CreateServiceOrderRequest request)
    {
        var serviceOrder = await _serviceOrderService.CreateAsync(request);

        if (serviceOrder is null)
        {
            return NotFound($"Vehicle with id {request.VehicleId} was not found.");
        }

        return CreatedAtAction(nameof(GetById), new { id = serviceOrder.ServiceOrderId }, serviceOrder);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateServiceOrderStatusRequest request)
    {
        var updated = await _serviceOrderService.UpdateStatusAsync(id, request);

        if (!updated)
        {
            return BadRequest("Service order could not be updated. It may not exist or may already be closed.");
        }

        return NoContent();
    }

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id)
    {
        var closed = await _serviceOrderService.CloseAsync(id);

        if (!closed)
        {
            return BadRequest("Service order could not be closed. Make sure it exists and all operations are completed or cancelled.");
        }

        return NoContent();
    }
}