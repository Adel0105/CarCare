using CarCare.Application.Expenses.Interfaces;
using CarCare.Application.MaintenanceRecords.Interfaces;
using CarCare.Application.Vehicles.Interfaces;
using CarCare.Infrastructure.Expenses;
using CarCare.Infrastructure.MaintenanceRecords;
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
        services.AddScoped<IExpenseReadStore, ExpenseReadStore>();
        services.AddScoped<IExpenseWriteStore, ExpenseWriteStore>();
        services.AddScoped<IMaintenanceRecordReadStore, MaintenanceRecordReadStore>();
        services.AddScoped<IMaintenanceRecordWriteStore, MaintenanceRecordWriteStore>();

        return services;
    }
}
