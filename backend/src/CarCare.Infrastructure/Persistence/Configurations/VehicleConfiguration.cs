using CarCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(vehicle => vehicle.Id);

        builder.Property(vehicle => vehicle.Make)
            .IsRequired();

        builder.Property(vehicle => vehicle.Model)
            .IsRequired();

        builder.Property(vehicle => vehicle.LicensePlate)
            .IsRequired();

        builder.HasIndex(vehicle => vehicle.LicensePlate)
            .IsUnique();

        builder.Property(vehicle => vehicle.Notes)
            .IsRequired(false);

        builder.Property(vehicle => vehicle.Year)
            .IsRequired();

        builder.Property(vehicle => vehicle.CurrentMileage)
            .IsRequired();

        builder.Property(vehicle => vehicle.CreatedAt)
            .IsRequired();
    }
}
