using CarCare.Application.Vehicles.Interfaces;
using CarCare.Infrastructure.Persistence;
using CarCare.Infrastructure.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Infrastructure;

public static class DependencyInjection
{
    private const string ConnectionStringName = "CarCare";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' is not configured.");

        services.AddDbContext<CarCareDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IVehicleReadStore, VehicleReadStore>();
        services.AddScoped<IVehicleWriteStore, VehicleWriteStore>();

        return services;
    }
}
