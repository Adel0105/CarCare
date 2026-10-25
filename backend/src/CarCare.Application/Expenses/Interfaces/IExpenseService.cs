namespace CarCare.Application.Expenses.Interfaces;

public interface IExpenseService
{
    Task<IReadOnlyList<ExpenseDto>?> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default);

    Task<ExpenseDto?> CreateAsync(
        int vehicleId,
        CreateExpenseDto request,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        int vehicleId,
        int expenseId,
        UpdateExpenseDto request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int vehicleId,
        int expenseId,
        CancellationToken cancellationToken = default);
}
