using CarCare.Application.MaintenanceReminders.Interfaces;
using CarCare.Application.Vehicles.Interfaces;
using CarCare.Domain.Entities;

namespace CarCare.Application.MaintenanceReminders;

public class MaintenanceReminderService : IMaintenanceReminderService
{
    private readonly IVehicleReadStore _vehicleReadStore;
    private readonly IMaintenanceReminderReadStore _maintenanceReminderReadStore;
    private readonly IMaintenanceReminderWriteStore _maintenanceReminderWriteStore;

    public MaintenanceReminderService(
        IVehicleReadStore vehicleReadStore,
        IMaintenanceReminderReadStore maintenanceReminderReadStore,
        IMaintenanceReminderWriteStore maintenanceReminderWriteStore)
    {
        _vehicleReadStore = vehicleReadStore;
        _maintenanceReminderReadStore = maintenanceReminderReadStore;
        _maintenanceReminderWriteStore = maintenanceReminderWriteStore;
    }

    public async Task<IReadOnlyList<MaintenanceReminderDto>?> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return null;
        }

        return await _maintenanceReminderReadStore.GetByVehicleIdAsync(vehicleId, cancellationToken);
    }

    public async Task<MaintenanceReminderDto?> CreateAsync(
        int vehicleId,
        CreateMaintenanceReminderDto request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return null;
        }

        var reminder = new MaintenanceReminder
        {
            VehicleId = vehicleId,
            Title = request.Title,
            DueDate = request.DueDate,
            DueMileage = request.DueMileage,
            IsCompleted = false,
            Notes = request.Notes,
        };

        await _maintenanceReminderWriteStore.AddAsync(reminder, cancellationToken);

        return new MaintenanceReminderDto
        {
            Id = reminder.Id,
            VehicleId = reminder.VehicleId,
            Title = reminder.Title,
            DueDate = reminder.DueDate,
            DueMileage = reminder.DueMileage,
            IsCompleted = reminder.IsCompleted,
            Notes = reminder.Notes,
        };
    }

    public async Task<bool> UpdateAsync(
        int vehicleId,
        int reminderId,
        UpdateMaintenanceReminderDto request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        return await _maintenanceReminderWriteStore.UpdateAsync(
            vehicleId,
            reminderId,
            request,
            cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int vehicleId,
        int reminderId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        return await _maintenanceReminderWriteStore.DeleteAsync(
            vehicleId,
            reminderId,
            cancellationToken);
    }
}
