using CarCare.Application.Vehicles.Interfaces;
using CarCare.Domain.Entities;
using CarCare.Infrastructure.Persistence;

namespace CarCare.Infrastructure.Vehicles;

public class VehicleWriteStore(CarCareDbContext dbContext) : IVehicleWriteStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
