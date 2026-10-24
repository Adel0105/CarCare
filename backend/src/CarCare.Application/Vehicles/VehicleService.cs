using CarCare.Application.Vehicles.Interfaces;
using CarCare.Domain.Entities;

namespace CarCare.Application.Vehicles;

public class VehicleService : IVehicleService
{
    private readonly IVehicleReadStore _vehicleReadStore;
    private readonly IVehicleWriteStore _vehicleWriteStore;

    public VehicleService(IVehicleReadStore vehicleReadStore, IVehicleWriteStore vehicleWriteStore)
    {
        _vehicleReadStore = vehicleReadStore;
        _vehicleWriteStore = vehicleWriteStore;
    }

    public Task<IReadOnlyList<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _vehicleReadStore.GetAllAsync(cancellationToken);
    }

    public Task<VehicleDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _vehicleReadStore.GetByIdAsync(id, cancellationToken);
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleDto request, CancellationToken cancellationToken = default)
    {
        var vehicle = new Vehicle
        {
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            LicensePlate = request.LicensePlate,
            CurrentMileage = request.CurrentMileage,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
        };

        await _vehicleWriteStore.AddAsync(vehicle, cancellationToken);

        return new VehicleDto
        {
            Id = vehicle.Id,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            LicensePlate = vehicle.LicensePlate,
            CurrentMileage = vehicle.CurrentMileage,
            Notes = vehicle.Notes,
        };
    }

    public Task<bool> UpdateAsync(int id, UpdateVehicleDto request, CancellationToken cancellationToken = default)
    {
        return _vehicleWriteStore.UpdateAsync(id, request, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return _vehicleWriteStore.DeleteAsync(id, cancellationToken);
    }
}
