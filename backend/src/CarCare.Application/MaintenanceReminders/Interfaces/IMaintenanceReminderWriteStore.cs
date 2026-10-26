using CarCare.Domain.Entities;

namespace CarCare.Application.MaintenanceReminders.Interfaces;

public interface IMaintenanceReminderWriteStore
{
    Task AddAsync(MaintenanceReminder reminder, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        int vehicleId,
        int reminderId,
        UpdateMaintenanceReminderDto request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int vehicleId,
        int reminderId,
        CancellationToken cancellationToken = default);
}
