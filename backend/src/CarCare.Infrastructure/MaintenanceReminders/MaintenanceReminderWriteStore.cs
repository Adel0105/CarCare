using CarCare.Application.MaintenanceReminders;
using CarCare.Application.MaintenanceReminders.Interfaces;
using CarCare.Domain.Entities;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.MaintenanceReminders;

public class MaintenanceReminderWriteStore(CarCareDbContext dbContext) : IMaintenanceReminderWriteStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task AddAsync(MaintenanceReminder reminder, CancellationToken cancellationToken = default)
    {
        await _dbContext.MaintenanceReminders.AddAsync(reminder, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        int vehicleId,
        int reminderId,
        UpdateMaintenanceReminderDto request,
        CancellationToken cancellationToken = default)
    {
        var reminder = await _dbContext.MaintenanceReminders
            .FirstOrDefaultAsync(
                entity => entity.Id == reminderId && entity.VehicleId == vehicleId,
                cancellationToken);

        if (reminder is null)
        {
            return false;
        }

        reminder.Title = request.Title;
        reminder.DueDate = request.DueDate;
        reminder.DueMileage = request.DueMileage;
        reminder.Notes = request.Notes;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(
        int vehicleId,
        int reminderId,
        CancellationToken cancellationToken = default)
    {
        var reminder = await _dbContext.MaintenanceReminders
            .FirstOrDefaultAsync(
                entity => entity.Id == reminderId && entity.VehicleId == vehicleId,
                cancellationToken);

        if (reminder is null)
        {
            return false;
        }

        _dbContext.MaintenanceReminders.Remove(reminder);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
