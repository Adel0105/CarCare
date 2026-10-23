using CarCare.Domain.Entities;

namespace CarCare.Application.Vehicles.Interfaces;

public interface IVehicleWriteStore
{
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
}
