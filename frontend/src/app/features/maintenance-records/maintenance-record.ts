export enum MaintenanceType {
  Service = 0,
  Repair = 1,
  Other = 2,
}

export interface MaintenanceRecord {
  id: number;
  vehicleId: number;
  date: string;
  mileage: number;
  title: string;
  type: MaintenanceType;
  description: string | null;
  cost: number;
}

export interface CreateMaintenanceRecord {
  date: string;
  mileage: number;
  title: string;
  type: MaintenanceType;
  description: string | null;
  cost: number;
}

/** Same body shape as create; matches backend UpdateMaintenanceRecordDto. */
export type UpdateMaintenanceRecord = CreateMaintenanceRecord;
