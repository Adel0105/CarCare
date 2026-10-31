import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CreateMaintenanceRecord, MaintenanceRecord } from './maintenance-record';

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
}
