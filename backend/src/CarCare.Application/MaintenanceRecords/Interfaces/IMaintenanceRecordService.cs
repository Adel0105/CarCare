namespace CarCare.Application.MaintenanceRecords.Interfaces;

public interface IMaintenanceRecordService
{
    Task<IReadOnlyList<MaintenanceRecordDto>?> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default);

    Task<MaintenanceRecordDto?> CreateAsync(
        int vehicleId,
        CreateMaintenanceRecordDto request,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        int vehicleId,
        int maintenanceRecordId,
        UpdateMaintenanceRecordDto request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int vehicleId,
        int maintenanceRecordId,
        CancellationToken cancellationToken = default);
}
