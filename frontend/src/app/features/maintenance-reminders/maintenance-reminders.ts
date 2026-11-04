import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { CreateMaintenanceReminder, MaintenanceReminder } from './maintenance-reminder';
import { MaintenanceReminderService } from './maintenance-reminder.service';

type MaintenanceRemindersViewState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'success'; reminders: MaintenanceReminder[] };

interface MaintenanceReminderFormModel {
  title: string;
  dueDate: string;
  dueMileage: number | null;
  notes: string;
}

interface FormMessage {
  type: 'success' | 'error';
  text: string;
}

@Component({
  selector: 'app-maintenance-reminders',
  imports: [FormsModule],
  templateUrl: './maintenance-reminders.html',
  styleUrl: './maintenance-reminders.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MaintenanceReminders implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly maintenanceReminderService = inject(MaintenanceReminderService);

  protected readonly form: MaintenanceReminderFormModel = {
    title: '',
    dueDate: '',
    dueMileage: null,
    notes: '',
  };

  private vehicleId: number | null = null;

  protected readonly viewState = signal<MaintenanceRemindersViewState>({ status: 'loading' });
  protected readonly formMessage = signal<FormMessage | null>(null);
  protected readonly isSubmitting = signal(false);
  protected readonly deletingReminderId = signal<number | null>(null);
  protected readonly editingReminder = signal<MaintenanceReminder | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.vehicleId = Number(params.get('vehicleId'));
      this.loadReminders();
    });
  }

  protected startEditing(reminder: MaintenanceReminder): void {
    this.editingReminder.set(reminder);
    this.form.title = reminder.title;
    this.form.dueDate = reminder.dueDate ?? '';
    this.form.dueMileage = reminder.dueMileage;
    this.form.notes = reminder.notes ?? '';
    this.formMessage.set(null);
  }

  protected cancelEditing(): void {
    this.editingReminder.set(null);
    this.resetForm();
    this.formMessage.set(null);
  }

  protected deleteReminder(reminder: MaintenanceReminder): void {
    if (this.deletingReminderId() !== null || this.isSubmitting()) {
      return;
    }

    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.formMessage.set({
        type: 'error',
        text: 'Greška pri brisanju podsjetnika. Pokušaj ponovo.',
      });
      return;
    }

    const confirmed = confirm(
      `Jesi li siguran da želiš obrisati podsjetnik „${reminder.title}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.deletingReminderId.set(reminder.id);
    this.formMessage.set(null);

    this.maintenanceReminderService.delete(this.vehicleId, reminder.id).subscribe({
      next: () => {
        if (this.editingReminder()?.id === reminder.id) {
          this.cancelEditing();
        }

        this.deletingReminderId.set(null);
        this.formMessage.set({
          type: 'success',
          text: 'Podsjetnik je uspješno obrisan.',
        });
        this.loadReminders();
      },
      error: () => {
        this.deletingReminderId.set(null);
        this.formMessage.set({
          type: 'error',
          text: 'Greška pri brisanju podsjetnika. Pokušaj ponovo.',
        });
      },
    });
  }

  protected onSubmit(): void {
    if (this.isSubmitting() || this.deletingReminderId() !== null) {
      return;
    }

    const dueDate = this.form.dueDate.trim() ? this.form.dueDate.trim() : null;
    const dueMileage = this.form.dueMileage;

    if (!this.form.title.trim() || (dueMileage !== null && dueMileage < 0)) {
      this.formMessage.set({
        type: 'error',
        text: 'Popuni sva obavezna polja ispravno.',
      });
      return;
    }

    if (dueDate === null && dueMileage === null) {
      this.formMessage.set({
        type: 'error',
        text: 'Unesi rok kao datum i/ili kilometražu.',
      });
      return;
    }

    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.formMessage.set({
        type: 'error',
        text: 'Greška pri dodavanju podsjetnika. Pokušaj ponovo.',
      });
      return;
    }

    const payload: CreateMaintenanceReminder = {
      title: this.form.title.trim(),
      dueDate,
      dueMileage,
      notes: this.form.notes.trim() ? this.form.notes.trim() : null,
    };

    const editing = this.editingReminder();

    this.isSubmitting.set(true);
    this.formMessage.set(null);

    if (editing) {
      this.maintenanceReminderService.update(this.vehicleId, editing.id, payload).subscribe({
        next: () => {
          this.editingReminder.set(null);
          this.resetForm();
          this.isSubmitting.set(false);
          this.formMessage.set({
            type: 'success',
            text: 'Podsjetnik je uspješno ažuriran.',
          });
          this.loadReminders();
        },
        error: () => {
          this.isSubmitting.set(false);
          this.formMessage.set({
            type: 'error',
            text: 'Greška pri ažuriranju podsjetnika. Pokušaj ponovo.',
          });
        },
      });
      return;
    }

    this.maintenanceReminderService.create(this.vehicleId, payload).subscribe({
      next: () => {
        this.resetForm();
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'success',
          text: 'Podsjetnik je uspješno dodan.',
        });
        this.loadReminders();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'error',
          text: 'Greška pri dodavanju podsjetnika. Pokušaj ponovo.',
        });
      },
    });
  }

  protected statusLabel(isCompleted: boolean): string {
    return isCompleted ? 'Završen' : 'Na čekanju';
  }

  private loadReminders(): void {
    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.viewState.set({ status: 'error' });
      return;
    }

    this.viewState.set({ status: 'loading' });

    this.maintenanceReminderService.getByVehicleId(this.vehicleId).subscribe({
      next: (reminders) => {
        this.viewState.set({ status: 'success', reminders });
      },
      error: () => {
        this.viewState.set({ status: 'error' });
      },
    });
  }

  private resetForm(): void {
    this.form.title = '';
    this.form.dueDate = '';
    this.form.dueMileage = null;
    this.form.notes = '';
  }
}
