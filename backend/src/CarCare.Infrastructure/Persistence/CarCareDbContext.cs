using CarCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Persistence;

public sealed class CarCareDbContext(DbContextOptions<CarCareDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();

    public DbSet<Expense> Expenses => Set<Expense>();

    public DbSet<MaintenanceReminder> MaintenanceReminders => Set<MaintenanceReminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarCareDbContext).Assembly);
    }
}
