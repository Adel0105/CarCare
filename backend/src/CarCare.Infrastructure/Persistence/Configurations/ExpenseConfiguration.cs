using CarCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(expense => expense.Id);

        builder.Property(expense => expense.VehicleId)
            .IsRequired();

        builder.Property(expense => expense.Date)
            .IsRequired();

        builder.Property(expense => expense.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(expense => expense.Category)
            .IsRequired();

        builder.Property(expense => expense.Description)
            .IsRequired(false);

        builder.Property(expense => expense.Mileage)
            .IsRequired(false);

        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(expense => expense.VehicleId)
            .IsRequired();
    }
}
