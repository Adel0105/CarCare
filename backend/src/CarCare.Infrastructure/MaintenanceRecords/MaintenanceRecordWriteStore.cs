using CarCare.Application.MaintenanceRecords;
using CarCare.Application.MaintenanceRecords.Interfaces;
using CarCare.Domain.Entities;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.MaintenanceRecords;

public class MaintenanceRecordWriteStore(CarCareDbContext dbContext) : IMaintenanceRecordWriteStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task AddAsync(MaintenanceRecord record, CancellationToken cancellationToken = default)
    {
        await _dbContext.MaintenanceRecords.AddAsync(record, cancellationToken);

        var vehicle = await _dbContext.Vehicles
            .FirstOrDefaultAsync(entity => entity.Id == record.VehicleId, cancellationToken);

        if (vehicle is not null && record.Mileage > vehicle.CurrentMileage)
        {
            vehicle.CurrentMileage = record.Mileage;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        int vehicleId,
        int maintenanceRecordId,
        UpdateMaintenanceRecordDto request,
        CancellationToken cancellationToken = default)
    {
        var record = await _dbContext.MaintenanceRecords
            .FirstOrDefaultAsync(
                entity => entity.Id == maintenanceRecordId && entity.VehicleId == vehicleId,
                cancellationToken);

        if (record is null)
        {
            return false;
        }

        record.Date = request.Date;
        record.Mileage = request.Mileage;
        record.Title = request.Title;
        record.Type = request.Type;
        record.Description = request.Description;
        record.Cost = request.Cost;

        var vehicle = await _dbContext.Vehicles
            .FirstOrDefaultAsync(entity => entity.Id == vehicleId, cancellationToken);

        if (vehicle is not null && request.Mileage > vehicle.CurrentMileage)
        {
            vehicle.CurrentMileage = request.Mileage;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(
        int vehicleId,
        int maintenanceRecordId,
        CancellationToken cancellationToken = default)
    {
        var record = await _dbContext.MaintenanceRecords
            .FirstOrDefaultAsync(
                entity => entity.Id == maintenanceRecordId && entity.VehicleId == vehicleId,
                cancellationToken);

        if (record is null)
        {
            return false;
        }

        _dbContext.MaintenanceRecords.Remove(record);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
