using CarCare.Domain.Enums;

namespace CarCare.Application.Dashboard;

public class DashboardExpenseCategoryDto
{
    public ExpenseCategory Category { get; set; }

    public decimal Amount { get; set; }
}
