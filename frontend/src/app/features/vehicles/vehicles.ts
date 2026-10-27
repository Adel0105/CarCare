import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { Vehicle } from './vehicle';
import { VehicleService } from './vehicle.service';

@Component({
  selector: 'app-vehicles',
  imports: [AsyncPipe],
  templateUrl: './vehicles.html',
  styleUrl: './vehicles.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Vehicles {
  private readonly vehicleService = inject(VehicleService);

  protected readonly vehicles$: Observable<Vehicle[]> = this.vehicleService.getAll();
}
