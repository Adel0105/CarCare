import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  CreateMaintenanceRecord,
  MaintenanceRecord,
  UpdateMaintenanceRecord,
} from './maintenance-record';

@Injectable({ providedIn: 'root' })
export class MaintenanceRecordService {
  private readonly http = inject(HttpClient);

  getByVehicleId(vehicleId: number): Observable<MaintenanceRecord[]> {
    return this.http.get<MaintenanceRecord[]>(
      `/api/vehicles/${vehicleId}/maintenance-records`,
    );
  }

  create(vehicleId: number, payload: CreateMaintenanceRecord): Observable<MaintenanceRecord> {
    return this.http.post<MaintenanceRecord>(
      `/api/vehicles/${vehicleId}/maintenance-records`,
      payload,
    );
  }

  update(
    vehicleId: number,
    maintenanceRecordId: number,
    payload: UpdateMaintenanceRecord,
  ): Observable<MaintenanceRecord> {
    return this.http.put<MaintenanceRecord>(
      `/api/vehicles/${vehicleId}/maintenance-records/${maintenanceRecordId}`,
      payload,
    );
  }

  delete(vehicleId: number, maintenanceRecordId: number): Observable<void> {
    return this.http.delete<void>(
      `/api/vehicles/${vehicleId}/maintenance-records/${maintenanceRecordId}`,
    );
  }
}
