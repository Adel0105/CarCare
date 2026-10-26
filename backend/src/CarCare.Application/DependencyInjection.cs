using CarCare.Application.Expenses;
using CarCare.Application.Expenses.Interfaces;
using CarCare.Application.MaintenanceRecords;
using CarCare.Application.MaintenanceRecords.Interfaces;
using CarCare.Application.MaintenanceReminders;
using CarCare.Application.MaintenanceReminders.Interfaces;
using CarCare.Application.Vehicles;
using CarCare.Application.Vehicles.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IMaintenanceRecordService, MaintenanceRecordService>();
        services.AddScoped<IMaintenanceReminderService, MaintenanceReminderService>();

        return services;
    }
}
