using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Persistence;

public sealed class CarCareDbContext(DbContextOptions<CarCareDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarCareDbContext).Assembly);
    }
}
