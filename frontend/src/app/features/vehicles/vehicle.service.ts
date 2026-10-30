import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CreateVehicle, UpdateVehicle, Vehicle } from './vehicle';

@Injectable({ providedIn: 'root' })
export class VehicleService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>('/api/vehicles');
  }

  create(vehicle: CreateVehicle): Observable<Vehicle> {
    return this.http.post<Vehicle>('/api/vehicles', vehicle);
  }

  update(id: number, vehicle: UpdateVehicle): Observable<void> {
    return this.http.put<void>(`/api/vehicles/${id}`, vehicle);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`/api/vehicles/${id}`);
  }
}
