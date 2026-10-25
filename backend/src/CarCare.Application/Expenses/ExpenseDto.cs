using CarCare.Domain.Enums;

namespace CarCare.Application.Expenses;

public class ExpenseDto
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Amount { get; set; }

    public ExpenseCategory Category { get; set; }

    public string? Description { get; set; }

    public int? Mileage { get; set; }
}
