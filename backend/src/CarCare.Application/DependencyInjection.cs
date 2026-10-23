using CarCare.Application.Vehicles;
using CarCare.Application.Vehicles.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();

        return services;
    }
}
