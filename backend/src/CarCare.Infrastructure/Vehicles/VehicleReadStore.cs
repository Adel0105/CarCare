using CarCare.Application.Vehicles;
using CarCare.Application.Vehicles.Interfaces;
using CarCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Vehicles;

public class VehicleReadStore(CarCareDbContext dbContext) : IVehicleReadStore
{
    private readonly CarCareDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vehicles
            .Select(vehicle => new VehicleDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                CurrentMileage = vehicle.CurrentMileage,
                Notes = vehicle.Notes,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<VehicleDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vehicles
            .Where(vehicle => vehicle.Id == id)
            .Select(vehicle => new VehicleDto
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                LicensePlate = vehicle.LicensePlate,
                CurrentMileage = vehicle.CurrentMileage,
                Notes = vehicle.Notes,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
