using CarCare.Application.Vehicles;
using CarCare.Application.Vehicles.Interfaces;
using CarCare.Domain.Entities;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Vehicles;

public class VehicleWriteStore(CarCareDbContext dbContext) : IVehicleWriteStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(int id, UpdateVehicleDto request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _dbContext.Vehicles
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (vehicle is null)
        {
            return false;
        }

        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.LicensePlate = request.LicensePlate;
        vehicle.CurrentMileage = request.CurrentMileage;
        vehicle.Notes = request.Notes;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _dbContext.Vehicles
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (vehicle is null)
        {
            return false;
        }

        _dbContext.Vehicles.Remove(vehicle);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
