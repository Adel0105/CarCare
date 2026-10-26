using CarCare.Application.MaintenanceReminders;
using CarCare.Application.MaintenanceReminders.Interfaces;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.MaintenanceReminders;

public class MaintenanceReminderReadStore(CarCareDbContext dbContext) : IMaintenanceReminderReadStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<MaintenanceReminderDto>> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.MaintenanceReminders
            .Where(reminder => reminder.VehicleId == vehicleId)
            .Select(reminder => new MaintenanceReminderDto
            {
                Id = reminder.Id,
                VehicleId = reminder.VehicleId,
                Title = reminder.Title,
                DueDate = reminder.DueDate,
                DueMileage = reminder.DueMileage,
                IsCompleted = reminder.IsCompleted,
                Notes = reminder.Notes,
            })
            .ToListAsync(cancellationToken);
    }
}
