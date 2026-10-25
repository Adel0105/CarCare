using CarCare.Application.Expenses;
using CarCare.Application.Expenses.Interfaces;
using CarCare.Domain.Entities;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Expenses;

public class ExpenseWriteStore(CarCareDbContext dbContext) : IExpenseWriteStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        await _dbContext.Expenses.AddAsync(expense, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        int vehicleId,
        int expenseId,
        UpdateExpenseDto request,
        CancellationToken cancellationToken = default)
    {
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(
                entity => entity.Id == expenseId && entity.VehicleId == vehicleId,
                cancellationToken);

        if (expense is null)
        {
            return false;
        }

        expense.Date = request.Date;
        expense.Amount = request.Amount;
        expense.Category = request.Category;
        expense.Description = request.Description;
        expense.Mileage = request.Mileage;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(
        int vehicleId,
        int expenseId,
        CancellationToken cancellationToken = default)
    {
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(
                entity => entity.Id == expenseId && entity.VehicleId == vehicleId,
                cancellationToken);

        if (expense is null)
        {
            return false;
        }

        _dbContext.Expenses.Remove(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
