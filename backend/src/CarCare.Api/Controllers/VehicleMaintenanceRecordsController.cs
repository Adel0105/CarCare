using CarCare.Application.MaintenanceRecords;
using CarCare.Application.MaintenanceRecords.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.Controllers;

[ApiController]
[Route("api/vehicles/{vehicleId:int}/maintenance-records")]
public class VehicleMaintenanceRecordsController : ControllerBase
{
    private readonly IMaintenanceRecordService _maintenanceRecordService;

    public VehicleMaintenanceRecordsController(IMaintenanceRecordService maintenanceRecordService)
    {
        _maintenanceRecordService = maintenanceRecordService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken)
    {
        var records = await _maintenanceRecordService.GetByVehicleIdAsync(vehicleId, cancellationToken);
        if (records is null)
        {
            return NotFound();
        }

        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        int vehicleId,
        [FromBody] CreateMaintenanceRecordDto request,
        CancellationToken cancellationToken)
    {
        var record = await _maintenanceRecordService.CreateAsync(vehicleId, request, cancellationToken);
        if (record is null)
        {
            return NotFound();
        }

        return StatusCode(StatusCodes.Status201Created, record);
    }

    [HttpPut("{maintenanceRecordId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int vehicleId,
        int maintenanceRecordId,
        [FromBody] UpdateMaintenanceRecordDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _maintenanceRecordService.UpdateAsync(
            vehicleId,
            maintenanceRecordId,
            request,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{maintenanceRecordId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int vehicleId,
        int maintenanceRecordId,
        CancellationToken cancellationToken)
    {
        var deleted = await _maintenanceRecordService.DeleteAsync(
            vehicleId,
            maintenanceRecordId,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
