using CarCare.Domain.Enums;

namespace CarCare.Application.Dashboard;

public class DashboardMaintenanceActivityDto
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public string VehicleLabel { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public string Title { get; set; } = string.Empty;

    public MaintenanceType Type { get; set; }

    public decimal Cost { get; set; }
}
