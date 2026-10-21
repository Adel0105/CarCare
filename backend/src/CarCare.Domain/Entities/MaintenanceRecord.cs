using CarCare.Domain.Enums;

namespace CarCare.Domain.Entities;

public class MaintenanceRecord
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public DateOnly Date { get; set; }

    public int Mileage { get; set; }

    public string Title { get; set; } = string.Empty;

    public MaintenanceType Type { get; set; }

    public string? Description { get; set; }

    public decimal Cost { get; set; }
}
