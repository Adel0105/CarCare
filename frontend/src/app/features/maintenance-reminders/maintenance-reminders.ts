import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { MaintenanceReminder } from './maintenance-reminder';
import { MaintenanceReminderService } from './maintenance-reminder.service';

type MaintenanceRemindersViewState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'success'; reminders: MaintenanceReminder[] };

@Component({
  selector: 'app-maintenance-reminders',
  templateUrl: './maintenance-reminders.html',
  styleUrl: './maintenance-reminders.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MaintenanceReminders implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly maintenanceReminderService = inject(MaintenanceReminderService);

  private vehicleId: number | null = null;

  protected readonly viewState = signal<MaintenanceRemindersViewState>({ status: 'loading' });

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.vehicleId = Number(params.get('vehicleId'));
      this.loadReminders();
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
}
