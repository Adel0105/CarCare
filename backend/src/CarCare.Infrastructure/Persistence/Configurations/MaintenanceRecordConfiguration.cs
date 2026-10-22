using CarCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence.Configurations;

public class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
{
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
        builder.HasKey(record => record.Id);

        builder.Property(record => record.VehicleId)
            .IsRequired();

        builder.Property(record => record.Date)
            .IsRequired();

        builder.Property(record => record.Mileage)
            .IsRequired();

        builder.Property(record => record.Title)
            .IsRequired();

        builder.Property(record => record.Type)
            .IsRequired();

        builder.Property(record => record.Description)
            .IsRequired(false);

        builder.Property(record => record.Cost)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(record => record.VehicleId)
            .IsRequired();
    }
}
