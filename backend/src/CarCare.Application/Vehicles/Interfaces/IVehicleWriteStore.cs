using CarCare.Domain.Entities;

namespace CarCare.Application.Vehicles.Interfaces;

public interface IVehicleWriteStore
{
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, UpdateVehicleDto request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
