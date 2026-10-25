using CarCare.Application.Expenses.Interfaces;
using CarCare.Application.Vehicles.Interfaces;
using CarCare.Domain.Entities;

namespace CarCare.Application.Expenses;

public class ExpenseService : IExpenseService
{
    private readonly IVehicleReadStore _vehicleReadStore;
    private readonly IExpenseReadStore _expenseReadStore;
    private readonly IExpenseWriteStore _expenseWriteStore;

    public ExpenseService(
        IVehicleReadStore vehicleReadStore,
        IExpenseReadStore expenseReadStore,
        IExpenseWriteStore expenseWriteStore)
    {
        _vehicleReadStore = vehicleReadStore;
        _expenseReadStore = expenseReadStore;
        _expenseWriteStore = expenseWriteStore;
    }

    public async Task<IReadOnlyList<ExpenseDto>?> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return null;
        }

        return await _expenseReadStore.GetByVehicleIdAsync(vehicleId, cancellationToken);
    }

    public async Task<ExpenseDto?> CreateAsync(
        int vehicleId,
        CreateExpenseDto request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return null;
        }

        var expense = new Expense
        {
            VehicleId = vehicleId,
            Date = request.Date,
            Amount = request.Amount,
            Category = request.Category,
            Description = request.Description,
            Mileage = request.Mileage,
        };

        await _expenseWriteStore.AddAsync(expense, cancellationToken);

        return new ExpenseDto
        {
            Id = expense.Id,
            VehicleId = expense.VehicleId,
            Date = expense.Date,
            Amount = expense.Amount,
            Category = expense.Category,
            Description = expense.Description,
            Mileage = expense.Mileage,
        };
    }

    public async Task<bool> UpdateAsync(
        int vehicleId,
        int expenseId,
        UpdateExpenseDto request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        return await _expenseWriteStore.UpdateAsync(vehicleId, expenseId, request, cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int vehicleId,
        int expenseId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        return await _expenseWriteStore.DeleteAsync(vehicleId, expenseId, cancellationToken);
    }
}
