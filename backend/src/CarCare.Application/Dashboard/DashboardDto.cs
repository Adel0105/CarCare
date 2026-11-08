namespace CarCare.Application.Dashboard;

public class DashboardDto
{
    public int TotalVehicles { get; set; }

    public decimal TotalExpensesAmount { get; set; }

    public decimal CurrentMonthExpensesAmount { get; set; }

    public int ActiveRemindersCount { get; set; }

    public int OverdueRemindersCount { get; set; }

    public IReadOnlyList<DashboardMaintenanceActivityDto> RecentMaintenance { get; set; } = [];

    public IReadOnlyList<DashboardExpenseCategoryDto> ExpensesByCategory { get; set; } = [];

    public IReadOnlyList<DashboardMonthlyExpenseDto> MonthlyExpenses { get; set; } = [];
}
