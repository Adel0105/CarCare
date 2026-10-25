using CarCare.Application.Expenses;
using CarCare.Application.Expenses.Interfaces;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Expenses;

public class ExpenseReadStore(CarCareDbContext dbContext) : IExpenseReadStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<ExpenseDto>> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Expenses
            .Where(expense => expense.VehicleId == vehicleId)
            .Select(expense => new ExpenseDto
            {
                Id = expense.Id,
                VehicleId = expense.VehicleId,
                Date = expense.Date,
                Amount = expense.Amount,
                Category = expense.Category,
                Description = expense.Description,
                Mileage = expense.Mileage,
            })
            .ToListAsync(cancellationToken);
    }
}
