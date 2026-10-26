using CarCare.Application.MaintenanceReminders;
using CarCare.Application.MaintenanceReminders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.Controllers;

[ApiController]
[Route("api/vehicles/{vehicleId:int}/maintenance-reminders")]
public class VehicleMaintenanceRemindersController : ControllerBase
{
    private readonly IMaintenanceReminderService _maintenanceReminderService;

    public VehicleMaintenanceRemindersController(IMaintenanceReminderService maintenanceReminderService)
    {
        _maintenanceReminderService = maintenanceReminderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken)
    {
        var reminders = await _maintenanceReminderService.GetByVehicleIdAsync(vehicleId, cancellationToken);
        if (reminders is null)
        {
            return NotFound();
        }

        return Ok(reminders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        int vehicleId,
        [FromBody] CreateMaintenanceReminderDto request,
        CancellationToken cancellationToken)
    {
        var reminder = await _maintenanceReminderService.CreateAsync(vehicleId, request, cancellationToken);
        if (reminder is null)
        {
            return NotFound();
        }

        return StatusCode(StatusCodes.Status201Created, reminder);
    }

    [HttpPut("{reminderId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int vehicleId,
        int reminderId,
        [FromBody] UpdateMaintenanceReminderDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _maintenanceReminderService.UpdateAsync(
            vehicleId,
            reminderId,
            request,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{reminderId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int vehicleId,
        int reminderId,
        CancellationToken cancellationToken)
    {
        var deleted = await _maintenanceReminderService.DeleteAsync(
            vehicleId,
            reminderId,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
