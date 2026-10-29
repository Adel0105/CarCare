import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, catchError, map, of, startWith, switchMap } from 'rxjs';

import { CreateVehicle, Vehicle } from './vehicle';
import { VehicleService } from './vehicle.service';

type VehiclesViewState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'success'; vehicles: Vehicle[] };

interface VehicleFormModel {
  make: string;
  model: string;
  year: number | null;
  licensePlate: string;
  currentMileage: number | null;
  notes: string;
}

interface FormMessage {
  type: 'success' | 'error';
  text: string;
}

@Component({
  selector: 'app-vehicles',
  imports: [AsyncPipe, FormsModule],
  templateUrl: './vehicles.html',
  styleUrl: './vehicles.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Vehicles {
  private readonly vehicleService = inject(VehicleService);
  private readonly reload$ = new BehaviorSubject<void>(undefined);

  protected readonly form: VehicleFormModel = {
    make: '',
    model: '',
    year: null,
    licensePlate: '',
    currentMileage: null,
    notes: '',
  };

  protected readonly isSubmitting = signal(false);
  protected readonly formMessage = signal<FormMessage | null>(null);

  protected readonly vehicles$: Observable<VehiclesViewState> = this.reload$.pipe(
    switchMap(() =>
      this.vehicleService.getAll().pipe(
        map((vehicles): VehiclesViewState => ({ status: 'success', vehicles })),
        startWith({ status: 'loading' } as VehiclesViewState),
        catchError((): Observable<VehiclesViewState> => of({ status: 'error' })),
      ),
    ),
  );

  protected onSubmit(): void {
    if (this.isSubmitting()) {
      return;
    }

    if (
      !this.form.make.trim() ||
      !this.form.model.trim() ||
      this.form.year === null ||
      !this.form.licensePlate.trim() ||
      this.form.currentMileage === null
    ) {
      this.formMessage.set({
        type: 'error',
        text: 'Popuni sva obavezna polja.',
      });
      return;
    }

    const payload: CreateVehicle = {
      make: this.form.make.trim(),
      model: this.form.model.trim(),
      year: this.form.year,
      licensePlate: this.form.licensePlate.trim(),
      currentMileage: this.form.currentMileage,
      notes: this.form.notes.trim() ? this.form.notes.trim() : null,
    };

    this.isSubmitting.set(true);
    this.formMessage.set(null);

    this.vehicleService.create(payload).subscribe({
      next: () => {
        this.resetForm();
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'success',
          text: 'Vozilo je uspješno dodano.',
        });
        this.reload$.next();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'error',
          text: 'Greška pri dodavanju vozila. Pokušaj ponovo.',
        });
      },
    });
  }

  private resetForm(): void {
    this.form.make = '';
    this.form.model = '';
    this.form.year = null;
    this.form.licensePlate = '';
    this.form.currentMileage = null;
    this.form.notes = '';
  }
}
