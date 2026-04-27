using AutoServiceManager.Api.DTOs.Operations;
using AutoServiceManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceManager.Api.Controllers;

[ApiController]
[Route("api")]
public class OperationsController : ControllerBase
{
    private readonly IOperationService _operationService;

    public OperationsController(IOperationService operationService)
    {
        _operationService = operationService;
    }

    [HttpGet("service-orders/{serviceOrderId:int}/operations")]
    public async Task<ActionResult<IEnumerable<OperationDto>>> GetByServiceOrderId(int serviceOrderId)
    {
        var operations = await _operationService.GetByServiceOrderIdAsync(serviceOrderId);

        return Ok(operations);
    }

    [HttpGet("operations/{id:int}")]
    public async Task<ActionResult<OperationDto>> GetById(int id)
    {
        var operation = await _operationService.GetByIdAsync(id);

        if (operation is null)
        {
            return NotFound();
        }

        return Ok(operation);
    }

    [HttpPost("service-orders/{serviceOrderId:int}/operations")]
    public async Task<ActionResult<OperationDto>> Create(int serviceOrderId, CreateOperationRequest request)
    {
        var operation = await _operationService.CreateAsync(serviceOrderId, request);

        if (operation is null)
        {
            return BadRequest("Operation could not be created. Check that the service order exists, is not closed, and the technician is active.");
        }

        return CreatedAtAction(nameof(GetById), new { id = operation.ServiceOrderOperationId }, operation);
    }

    [HttpPut("operations/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateOperationRequest request)
    {
        var updated = await _operationService.UpdateAsync(id, request);

        if (!updated)
        {
            return BadRequest("Operation could not be updated. Check that it exists, the service order is not closed, and the technician is active.");
        }

        return NoContent();
    }

    [HttpDelete("operations/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _operationService.DeleteAsync(id);

        if (!deleted)
        {
            return BadRequest("Operation could not be deleted. Check that it exists and the service order is not closed.");
        }

        return NoContent();
    }
}