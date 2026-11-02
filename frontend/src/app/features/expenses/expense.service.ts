import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CreateExpense, Expense, UpdateExpense } from './expense';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private readonly http = inject(HttpClient);

  getByVehicleId(vehicleId: number): Observable<Expense[]> {
    return this.http.get<Expense[]>(`/api/vehicles/${vehicleId}/expenses`);
  }

  create(vehicleId: number, payload: CreateExpense): Observable<Expense> {
    return this.http.post<Expense>(`/api/vehicles/${vehicleId}/expenses`, payload);
  }

  update(vehicleId: number, expenseId: number, payload: UpdateExpense): Observable<void> {
    return this.http.put<void>(`/api/vehicles/${vehicleId}/expenses/${expenseId}`, payload);
  }

  delete(vehicleId: number, expenseId: number): Observable<void> {
    return this.http.delete<void>(`/api/vehicles/${vehicleId}/expenses/${expenseId}`);
  }
}
