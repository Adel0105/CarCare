using CarCare.Application.Vehicles;
using CarCare.Application.Vehicles.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleService.GetAllAsync(cancellationToken);
        return Ok(vehicles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id, cancellationToken);
        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateVehicleDto request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.CreateAsync(request, cancellationToken);
        if (vehicle is null)
        {
            return Conflict();
        }

        return StatusCode(StatusCodes.Status201Created, vehicle);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] UpdateVehicleDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _vehicleService.UpdateAsync(id, request, cancellationToken);
        if (updated is null)
        {
            return Conflict();
        }

        if (updated == false)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _vehicleService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
