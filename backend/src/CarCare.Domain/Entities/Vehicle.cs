namespace CarCare.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string LicensePlate { get; set; } = string.Empty;

    public int CurrentMileage { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}
