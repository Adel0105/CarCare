using CarCare.Application.MaintenanceRecords.Interfaces;
using CarCare.Application.Vehicles.Interfaces;
using CarCare.Domain.Entities;

namespace CarCare.Application.MaintenanceRecords;

public class MaintenanceRecordService : IMaintenanceRecordService
{
    private readonly IVehicleReadStore _vehicleReadStore;
    private readonly IMaintenanceRecordReadStore _maintenanceRecordReadStore;
    private readonly IMaintenanceRecordWriteStore _maintenanceRecordWriteStore;

    public MaintenanceRecordService(
        IVehicleReadStore vehicleReadStore,
        IMaintenanceRecordReadStore maintenanceRecordReadStore,
        IMaintenanceRecordWriteStore maintenanceRecordWriteStore)
    {
        _vehicleReadStore = vehicleReadStore;
        _maintenanceRecordReadStore = maintenanceRecordReadStore;
        _maintenanceRecordWriteStore = maintenanceRecordWriteStore;
    }

    public async Task<IReadOnlyList<MaintenanceRecordDto>?> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return null;
        }

        return await _maintenanceRecordReadStore.GetByVehicleIdAsync(vehicleId, cancellationToken);
    }

    public async Task<MaintenanceRecordDto?> CreateAsync(
        int vehicleId,
        CreateMaintenanceRecordDto request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return null;
        }

        var record = new MaintenanceRecord
        {
            VehicleId = vehicleId,
            Date = request.Date,
            Mileage = request.Mileage,
            Title = request.Title,
            Type = request.Type,
            Description = request.Description,
            Cost = request.Cost,
        };

        await _maintenanceRecordWriteStore.AddAsync(record, cancellationToken);

        return new MaintenanceRecordDto
        {
            Id = record.Id,
            VehicleId = record.VehicleId,
            Date = record.Date,
            Mileage = record.Mileage,
            Title = record.Title,
            Type = record.Type,
            Description = record.Description,
            Cost = record.Cost,
        };
    }

    public async Task<bool> UpdateAsync(
        int vehicleId,
        int maintenanceRecordId,
        UpdateMaintenanceRecordDto request,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        return await _maintenanceRecordWriteStore.UpdateAsync(
            vehicleId,
            maintenanceRecordId,
            request,
            cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int vehicleId,
        int maintenanceRecordId,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleReadStore.GetByIdAsync(vehicleId, cancellationToken);
        if (vehicle is null)
        {
            return false;
        }

        return await _maintenanceRecordWriteStore.DeleteAsync(
            vehicleId,
            maintenanceRecordId,
            cancellationToken);
    }
}
