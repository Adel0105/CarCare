using CarCare.Application.MaintenanceRecords;
using CarCare.Application.MaintenanceRecords.Interfaces;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.MaintenanceRecords;

public class MaintenanceRecordReadStore(CarCareDbContext dbContext) : IMaintenanceRecordReadStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<MaintenanceRecordDto>> GetByVehicleIdAsync(
        int vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.MaintenanceRecords
            .Where(record => record.VehicleId == vehicleId)
            .Select(record => new MaintenanceRecordDto
            {
                Id = record.Id,
                VehicleId = record.VehicleId,
                Date = record.Date,
                Mileage = record.Mileage,
                Title = record.Title,
                Type = record.Type,
                Description = record.Description,
                Cost = record.Cost,
            })
            .ToListAsync(cancellationToken);
    }
}
