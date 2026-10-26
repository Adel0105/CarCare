using System.ComponentModel.DataAnnotations;

namespace CarCare.Application.MaintenanceReminders;

public class CreateMaintenanceReminderDto : IValidatableObject
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public DateOnly? DueDate { get; set; }

    [Range(0, int.MaxValue)]
    public int? DueMileage { get; set; }

    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            yield return new ValidationResult(
                "Title must not be empty or whitespace.",
                [nameof(Title)]);
        }

        if (DueDate is null && DueMileage is null)
        {
            yield return new ValidationResult(
                "At least one of DueDate or DueMileage must be provided.",
                [nameof(DueDate), nameof(DueMileage)]);
        }
    }
}
