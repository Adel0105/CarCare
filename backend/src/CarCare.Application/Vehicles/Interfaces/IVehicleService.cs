namespace CarCare.Application.Vehicles.Interfaces;

public interface IVehicleService
{
    Task<IReadOnlyList<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<VehicleDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<VehicleDto?> CreateAsync(CreateVehicleDto request, CancellationToken cancellationToken = default);

    Task<bool?> UpdateAsync(int id, UpdateVehicleDto request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
