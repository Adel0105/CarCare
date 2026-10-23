namespace CarCare.Application.Vehicles.Interfaces;

public interface IVehicleReadStore
{
    Task<IReadOnlyList<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
