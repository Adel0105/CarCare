using CarCare.Application.Dashboard;
using CarCare.Application.Dashboard.Interfaces;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Dashboard;

public class DashboardReadStore(CarCareDbContext dbContext) : IDashboardReadStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var monthStart = new DateOnly(today.Year, today.Month, 1);
        var nextMonthStart = monthStart.AddMonths(1);

        var totalVehicles = await _dbContext.Vehicles.CountAsync(cancellationToken);

        var totalExpensesAmount = await _dbContext.Expenses
            .SumAsync(expense => (decimal?)expense.Amount, cancellationToken) ?? 0m;

        var currentMonthExpensesAmount = await _dbContext.Expenses
            .Where(expense => expense.Date >= monthStart && expense.Date < nextMonthStart)
            .SumAsync(expense => (decimal?)expense.Amount, cancellationToken) ?? 0m;

        var activeRemindersCount = await _dbContext.MaintenanceReminders
            .CountAsync(reminder => !reminder.IsCompleted, cancellationToken);

        var overdueRemindersCount = await (
            from reminder in _dbContext.MaintenanceReminders
            join vehicle in _dbContext.Vehicles on reminder.VehicleId equals vehicle.Id
            where !reminder.IsCompleted
                && (
                    (reminder.DueDate != null && reminder.DueDate < today)
                    || (reminder.DueMileage != null && reminder.DueMileage <= vehicle.CurrentMileage)
                )
            select reminder.Id
        ).CountAsync(cancellationToken);

        var recentMaintenance = await (
            from record in _dbContext.MaintenanceRecords
            join vehicle in _dbContext.Vehicles on record.VehicleId equals vehicle.Id
            orderby record.Date descending, record.Id descending
            select new DashboardMaintenanceActivityDto
            {
                Id = record.Id,
                VehicleId = record.VehicleId,
                VehicleLabel = vehicle.Make + " " + vehicle.Model + " (" + vehicle.LicensePlate + ")",
                Date = record.Date,
                Title = record.Title,
                Type = record.Type,
                Cost = record.Cost,
            }
        )
            .Take(5)
            .ToListAsync(cancellationToken);

        return new DashboardDto
        {
            TotalVehicles = totalVehicles,
            TotalExpensesAmount = totalExpensesAmount,
            CurrentMonthExpensesAmount = currentMonthExpensesAmount,
            ActiveRemindersCount = activeRemindersCount,
            OverdueRemindersCount = overdueRemindersCount,
            RecentMaintenance = recentMaintenance,
        };
    }
}
