import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CreateVehicle, Vehicle } from './vehicle';

@Injectable({ providedIn: 'root' })
export class VehicleService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>('/api/vehicles');
  }

  create(vehicle: CreateVehicle): Observable<Vehicle> {
    return this.http.post<Vehicle>('/api/vehicles', vehicle);
  }
}
