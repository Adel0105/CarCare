namespace CarCare.Application.Expenses.Interfaces;

public interface IExpenseReadStore
{
    Task<IReadOnlyList<ExpenseDto>> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default);
}
