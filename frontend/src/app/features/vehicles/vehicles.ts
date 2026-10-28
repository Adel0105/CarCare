import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Observable, catchError, map, of, startWith } from 'rxjs';

import { Vehicle } from './vehicle';
import { VehicleService } from './vehicle.service';

type VehiclesViewState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'success'; vehicles: Vehicle[] };

@Component({
  selector: 'app-vehicles',
  imports: [AsyncPipe],
  templateUrl: './vehicles.html',
  styleUrl: './vehicles.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Vehicles {
  private readonly vehicleService = inject(VehicleService);

  protected readonly vehicles$: Observable<VehiclesViewState> = this.vehicleService.getAll().pipe(
    map((vehicles): VehiclesViewState => ({ status: 'success', vehicles })),
    startWith({ status: 'loading' } as VehiclesViewState),
    catchError((): Observable<VehiclesViewState> => of({ status: 'error' })),
  );
}
