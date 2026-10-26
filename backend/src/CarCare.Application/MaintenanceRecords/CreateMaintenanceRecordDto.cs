using System.ComponentModel.DataAnnotations;
using CarCare.Domain.Enums;

namespace CarCare.Application.MaintenanceRecords;

public class CreateMaintenanceRecordDto : IValidatableObject
{
    public DateOnly Date { get; set; }

    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public MaintenanceType Type { get; set; }

    public string? Description { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Cost { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date == default)
        {
            yield return new ValidationResult(
                "Date is required.",
                [nameof(Date)]);
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            yield return new ValidationResult(
                "Title must not be empty or whitespace.",
                [nameof(Title)]);
        }

        if (!Enum.IsDefined(Type))
        {
            yield return new ValidationResult(
                "Type is not a valid maintenance type.",
                [nameof(Type)]);
        }
    }
}
