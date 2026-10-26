using CarCare.Domain.Entities;

namespace CarCare.Application.MaintenanceRecords.Interfaces;

public interface IMaintenanceRecordWriteStore
{
    Task AddAsync(MaintenanceRecord record, CancellationToken cancellationToken = default);

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
