using CarCare.Domain.Entities;

namespace CarCare.Application.Expenses.Interfaces;

public interface IExpenseWriteStore
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken = default);

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
