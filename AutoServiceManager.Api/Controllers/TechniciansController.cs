using AutoServiceManager.Api.DTOs.Technicians;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoServiceManager.Api.Common;

namespace AutoServiceManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechniciansController : ControllerBase
{
    private readonly ITechnicianService _technicianService;

    public TechniciansController(ITechnicianService technicianService)
    {
        _technicianService = technicianService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TechnicianDto>>> GetAll([FromQuery] PagedRequest request)
    {
        var technicians = await _technicianService.GetAllAsync(request);

        return Ok(technicians);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TechnicianDto>> GetById(int id)
    {
        var technician = await _technicianService.GetByIdAsync(id);

        if (technician is null)
        {
            return NotFound();
        }

        return Ok(technician);
    }

    [HttpPost]
    public async Task<ActionResult<TechnicianDto>> Create(CreateTechnicianRequest request)
    {
        var technician = await _technicianService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = technician.TechnicianId }, technician);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTechnicianRequest request)
    {
        var updated = await _technicianService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var deactivated = await _technicianService.DeactivateAsync(id);

        if (!deactivated)
        {
            return NotFound();
        }

        return NoContent();
    }
}