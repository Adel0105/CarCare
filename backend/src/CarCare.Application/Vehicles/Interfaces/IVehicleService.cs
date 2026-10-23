namespace CarCare.Application.Vehicles.Interfaces;

public interface IVehicleService
{
    Task<IReadOnlyList<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<VehicleDto> CreateAsync(CreateVehicleDto request, CancellationToken cancellationToken = default);
}
