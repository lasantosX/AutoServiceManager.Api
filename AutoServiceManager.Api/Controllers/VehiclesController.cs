using AutoServiceManager.Api.DTOs.Vehicles;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Controllers;

[ApiController]
[Route("api")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet("customers/{customerId:int}/vehicles")]
    public async Task<ActionResult<PagedResult<VehicleDto>>> GetByCustomerId(
    int customerId,
    [FromQuery] PagedRequest request)
    {
        var vehicles = await _vehicleService.GetByCustomerIdAsync(customerId, request);

        return Ok(vehicles);
    }

    [HttpGet("vehicles/{id:int}")]
    public async Task<ActionResult<VehicleDto>> GetById(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);

        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost("customers/{customerId:int}/vehicles")]
    public async Task<ActionResult<VehicleDto>> Create(int customerId, CreateVehicleRequest request)
    {
        var vehicle = await _vehicleService.CreateAsync(customerId, request);

        if (vehicle is null)
        {
            return NotFound($"Customer with id {customerId} was not found.");
        }

        return CreatedAtAction(nameof(GetById), new { id = vehicle.VehicleId }, vehicle);
    }

    [HttpPut("vehicles/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateVehicleRequest request)
    {
        var updated = await _vehicleService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("vehicles/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _vehicleService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}