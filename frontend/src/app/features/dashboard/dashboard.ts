import { ExpenseCategory } from '../expenses/expense';
import { MaintenanceType } from '../maintenance-records/maintenance-record';

export interface DashboardMaintenanceActivity {
  id: number;
  vehicleId: number;
  vehicleLabel: string;
  date: string;
  title: string;
  type: MaintenanceType;
  cost: number;
}

export interface DashboardExpenseCategory {
  category: ExpenseCategory;
  amount: number;
}

export interface DashboardMonthlyExpense {
  year: number;
  month: number;
  amount: number;
}

export interface Dashboard {
  totalVehicles: number;
  totalExpensesAmount: number;
  currentMonthExpensesAmount: number;
  activeRemindersCount: number;
  overdueRemindersCount: number;
  recentMaintenance: DashboardMaintenanceActivity[];
  expensesByCategory: DashboardExpenseCategory[];
  monthlyExpenses: DashboardMonthlyExpense[];
}
