import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, map, of } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiStatus {
  private readonly http = inject(HttpClient);

  isAvailable(): Observable<boolean> {
    return this.http.get(`${environment.apiBaseUrl}/health`, { responseType: 'text' }).pipe(
      map(() => true),
      catchError(() => of(false)),
    );
  }
}
