namespace CarCare.Application.MaintenanceReminders.Interfaces;

public interface IMaintenanceReminderReadStore
{
    Task<IReadOnlyList<MaintenanceReminderDto>> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default);
}
