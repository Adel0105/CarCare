using CarCare.Application.Expenses;
using CarCare.Application.Expenses.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.Controllers;

[ApiController]
[Route("api/vehicles/{vehicleId:int}/expenses")]
public class VehicleExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public VehicleExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken)
    {
        var expenses = await _expenseService.GetByVehicleIdAsync(vehicleId, cancellationToken);
        if (expenses is null)
        {
            return NotFound();
        }

        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        int vehicleId,
        [FromBody] CreateExpenseDto request,
        CancellationToken cancellationToken)
    {
        var expense = await _expenseService.CreateAsync(vehicleId, request, cancellationToken);
        if (expense is null)
        {
            return NotFound();
        }

        return StatusCode(StatusCodes.Status201Created, expense);
    }

    [HttpPut("{expenseId:int}")]
    public async Task<IActionResult> UpdateAsync(
        int vehicleId,
        int expenseId,
        [FromBody] UpdateExpenseDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _expenseService.UpdateAsync(vehicleId, expenseId, request, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{expenseId:int}")]
    public async Task<IActionResult> DeleteAsync(
        int vehicleId,
        int expenseId,
        CancellationToken cancellationToken)
    {
        var deleted = await _expenseService.DeleteAsync(vehicleId, expenseId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
