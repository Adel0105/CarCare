using CarCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence.Configurations;

public class MaintenanceReminderConfiguration : IEntityTypeConfiguration<MaintenanceReminder>
{
    public void Configure(EntityTypeBuilder<MaintenanceReminder> builder)
    {
        builder.HasKey(reminder => reminder.Id);

        builder.Property(reminder => reminder.VehicleId)
            .IsRequired();

        builder.Property(reminder => reminder.Title)
            .IsRequired();

        builder.Property(reminder => reminder.DueDate)
            .IsRequired(false);

        builder.Property(reminder => reminder.DueMileage)
            .IsRequired(false);

        builder.Property(reminder => reminder.IsCompleted)
            .IsRequired();

        builder.Property(reminder => reminder.Notes)
            .IsRequired(false);

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(reminder => reminder.VehicleId)
            .IsRequired();
    }
}
