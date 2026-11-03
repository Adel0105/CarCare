import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import {
  CreateMaintenanceReminder,
  MaintenanceReminder,
  UpdateMaintenanceReminder,
} from './maintenance-reminder';

@Injectable({ providedIn: 'root' })
export class MaintenanceReminderService {
  private readonly http = inject(HttpClient);

  getByVehicleId(vehicleId: number): Observable<MaintenanceReminder[]> {
    return this.http.get<MaintenanceReminder[]>(
      `/api/vehicles/${vehicleId}/maintenance-reminders`,
    );
  }

  create(
    vehicleId: number,
    payload: CreateMaintenanceReminder,
  ): Observable<MaintenanceReminder> {
    return this.http.post<MaintenanceReminder>(
      `/api/vehicles/${vehicleId}/maintenance-reminders`,
      payload,
    );
  }

  update(
    vehicleId: number,
    reminderId: number,
    payload: UpdateMaintenanceReminder,
  ): Observable<void> {
    return this.http.put<void>(
      `/api/vehicles/${vehicleId}/maintenance-reminders/${reminderId}`,
      payload,
    );
  }

  delete(vehicleId: number, reminderId: number): Observable<void> {
    return this.http.delete<void>(
      `/api/vehicles/${vehicleId}/maintenance-reminders/${reminderId}`,
    );
  }
}
