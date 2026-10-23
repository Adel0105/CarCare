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

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateVehicleDto request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, vehicle);
    }
}
