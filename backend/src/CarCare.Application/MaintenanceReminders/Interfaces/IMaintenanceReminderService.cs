namespace CarCare.Application.MaintenanceReminders.Interfaces;

public interface IMaintenanceReminderService
{
    Task<IReadOnlyList<MaintenanceReminderDto>?> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default);

    Task<MaintenanceReminderDto?> CreateAsync(
        int vehicleId,
        CreateMaintenanceReminderDto request,
        CancellationToken cancellationToken = default);

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
