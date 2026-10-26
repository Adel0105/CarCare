namespace CarCare.Application.MaintenanceReminders;

public class MaintenanceReminderDto
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateOnly? DueDate { get; set; }

    public int? DueMileage { get; set; }

    public bool IsCompleted { get; set; }

    public string? Notes { get; set; }
}
