import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { CreateMaintenanceRecord, MaintenanceRecord, MaintenanceType } from './maintenance-record';
import { MaintenanceRecordService } from './maintenance-record.service';

type MaintenanceRecordsViewState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'success'; records: MaintenanceRecord[] };

interface MaintenanceRecordFormModel {
  date: string;
  mileage: number | null;
  title: string;
  type: MaintenanceType;
  description: string;
  cost: number | null;
}

interface FormMessage {
  type: 'success' | 'error';
  text: string;
}

@Component({
  selector: 'app-maintenance-records',
  imports: [FormsModule],
  templateUrl: './maintenance-records.html',
  styleUrl: './maintenance-records.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MaintenanceRecords implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly maintenanceRecordService = inject(MaintenanceRecordService);

  protected readonly MaintenanceType = MaintenanceType;

  protected readonly form: MaintenanceRecordFormModel = {
    date: '',
    mileage: null,
    title: '',
    type: MaintenanceType.Service,
    description: '',
    cost: null,
  };

  private vehicleId: number | null = null;

  protected readonly viewState = signal<MaintenanceRecordsViewState>({ status: 'loading' });
  protected readonly formMessage = signal<FormMessage | null>(null);
  protected readonly isSubmitting = signal(false);
  protected readonly deletingRecordId = signal<number | null>(null);
  protected readonly editingRecord = signal<MaintenanceRecord | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.vehicleId = Number(params.get('vehicleId'));
      this.loadRecords();
    });
  }

  protected startEditing(record: MaintenanceRecord): void {
    this.editingRecord.set(record);
    this.form.date = record.date;
    this.form.mileage = record.mileage;
    this.form.title = record.title;
    this.form.type = record.type;
    this.form.description = record.description ?? '';
    this.form.cost = record.cost;
    this.formMessage.set(null);
  }

  protected cancelEditing(): void {
    this.editingRecord.set(null);
    this.resetForm();
    this.formMessage.set(null);
  }

  protected deleteRecord(record: MaintenanceRecord): void {
    if (this.deletingRecordId() !== null || this.isSubmitting()) {
      return;
    }

    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.formMessage.set({
        type: 'error',
        text: 'Failed to delete record. Please try again.',
      });
      return;
    }

    const confirmed = confirm(
      `Are you sure you want to delete "${record.title}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.deletingRecordId.set(record.id);
    this.formMessage.set(null);

    this.maintenanceRecordService.delete(this.vehicleId, record.id).subscribe({
      next: () => {
        if (this.editingRecord()?.id === record.id) {
          this.editingRecord.set(null);
          this.resetForm();
        }

        this.deletingRecordId.set(null);
        this.formMessage.set({
          type: 'success',
          text: 'Maintenance record deleted successfully.',
        });
        this.loadRecords();
      },
      error: () => {
        this.deletingRecordId.set(null);
        this.formMessage.set({
          type: 'error',
          text: 'Failed to delete record. Please try again.',
        });
      },
    });
  }

  protected onSubmit(): void {
    if (this.isSubmitting() || this.deletingRecordId() !== null) {
      return;
    }

    if (
      !this.form.title.trim() ||
      !this.form.date ||
      this.form.mileage === null ||
      this.form.mileage < 0 ||
      !this.isValidType(this.form.type) ||
      this.form.cost === null ||
      this.form.cost < 0
    ) {
      this.formMessage.set({
        type: 'error',
        text: 'Please fill in all required fields correctly.',
      });
      return;
    }

    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.formMessage.set({
        type: 'error',
        text: 'Failed to add record. Please try again.',
      });
      return;
    }

    const payload: CreateMaintenanceRecord = {
      date: this.form.date,
      mileage: this.form.mileage,
      title: this.form.title.trim(),
      type: this.form.type,
      description: this.form.description.trim() ? this.form.description.trim() : null,
      cost: this.form.cost,
    };

    const editing = this.editingRecord();

    this.isSubmitting.set(true);
    this.formMessage.set(null);

    if (editing) {
      this.maintenanceRecordService.update(this.vehicleId, editing.id, payload).subscribe({
        next: () => {
          this.editingRecord.set(null);
          this.resetForm();
          this.isSubmitting.set(false);
          this.formMessage.set({
            type: 'success',
          text: 'Maintenance record updated successfully.',
        });
        this.loadRecords();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'error',
          text: 'Failed to update record. Please try again.',
        });
      },
    });
      return;
    }

    this.maintenanceRecordService.create(this.vehicleId, payload).subscribe({
      next: () => {
        this.resetForm();
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'success',
          text: 'Maintenance record added successfully.',
        });
        this.loadRecords();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'error',
          text: 'Failed to add record. Please try again.',
        });
      },
    });
  }

  protected typeLabel(type: MaintenanceType): string {
    switch (type) {
      case MaintenanceType.Service:
        return 'Service';
      case MaintenanceType.Repair:
        return 'Repair';
      case MaintenanceType.Other:
        return 'Other';
    }
  }

  private loadRecords(): void {
    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.viewState.set({ status: 'error' });
      return;
    }

    this.viewState.set({ status: 'loading' });

    this.maintenanceRecordService.getByVehicleId(this.vehicleId).subscribe({
      next: (records) => {
        this.viewState.set({ status: 'success', records });
      },
      error: () => {
        this.viewState.set({ status: 'error' });
      },
    });
  }

  private resetForm(): void {
    this.form.date = '';
    this.form.mileage = null;
    this.form.title = '';
    this.form.type = MaintenanceType.Service;
    this.form.description = '';
    this.form.cost = null;
  }

  private isValidType(type: MaintenanceType): boolean {
    return (
      type === MaintenanceType.Service ||
      type === MaintenanceType.Repair ||
      type === MaintenanceType.Other
    );
  }
}
