namespace CarCare.Application.MaintenanceRecords.Interfaces;

public interface IMaintenanceRecordReadStore
{
    Task<IReadOnlyList<MaintenanceRecordDto>> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default);
}
