export interface MaintenanceReminder {
  id: number;
  vehicleId: number;
  title: string;
  dueDate: string | null;
  dueMileage: number | null;
  isCompleted: boolean;
  notes: string | null;
}

export interface CreateMaintenanceReminder {
  title: string;
  dueDate: string | null;
  dueMileage: number | null;
  notes: string | null;
}

/** Same body shape as create; matches backend UpdateMaintenanceReminderDto. */
export type UpdateMaintenanceReminder = CreateMaintenanceReminder;
