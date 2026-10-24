using System.ComponentModel.DataAnnotations;

namespace CarCare.Application.Vehicles;

public class CreateVehicleDto : IValidatableObject
{
    [Required]
    public string Make { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int CurrentMileage { get; set; }

    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Make))
        {
            yield return new ValidationResult(
                "Make must not be empty or whitespace.",
                [nameof(Make)]);
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            yield return new ValidationResult(
                "Model must not be empty or whitespace.",
                [nameof(Model)]);
        }

        if (string.IsNullOrWhiteSpace(LicensePlate))
        {
            yield return new ValidationResult(
                "LicensePlate must not be empty or whitespace.",
                [nameof(LicensePlate)]);
        }

        var maxYear = DateTime.UtcNow.Year + 1;
        if (Year < 1900 || Year > maxYear)
        {
            yield return new ValidationResult(
                $"Year must be between 1900 and {maxYear}.",
                [nameof(Year)]);
        }
    }
}
