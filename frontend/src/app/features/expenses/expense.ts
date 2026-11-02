export enum ExpenseCategory {
  Fuel = 0,
  Insurance = 1,
  Registration = 2,
  Parking = 3,
  Other = 4,
}

export interface Expense {
  id: number;
  vehicleId: number;
  date: string;
  amount: number;
  category: ExpenseCategory;
  description: string | null;
  mileage: number | null;
}

export interface CreateExpense {
  date: string;
  amount: number;
  category: ExpenseCategory;
  description: string | null;
  mileage: number | null;
}

/** Same body shape as create; matches backend UpdateExpenseDto. */
export type UpdateExpense = CreateExpense;
