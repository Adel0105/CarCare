namespace CarCare.Domain.Entities;

public class MaintenanceReminder
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateOnly? DueDate { get; set; }

    public int? DueMileage { get; set; }

    public bool IsCompleted { get; set; } = false;

    public string? Notes { get; set; }
}
