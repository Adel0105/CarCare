import { Routes } from '@angular/router';

import { Shell } from './core/layout/shell/shell';

export const routes: Routes = [
  {
    path: '',
    component: Shell,
    children: [
      {
        path: '',
        title: 'CarCare',
        loadComponent: () => import('./features/home/home').then((m) => m.Home),
      },
      {
        path: 'vehicles',
        title: 'Moja vozila',
        loadComponent: () => import('./features/vehicles/vehicles').then((m) => m.Vehicles),
      },
      {
        path: 'vehicles/:vehicleId/maintenance-records',
        title: 'Održavanje',
        loadComponent: () =>
          import('./features/maintenance-records/maintenance-records').then(
            (m) => m.MaintenanceRecords,
          ),
      },
      {
        path: 'vehicles/:vehicleId/expenses',
        title: 'Troškovi',
        loadComponent: () => import('./features/expenses/expenses').then((m) => m.Expenses),
      },
      {
        path: 'vehicles/:vehicleId/maintenance-reminders',
        title: 'Podsjetnici',
        loadComponent: () =>
          import('./features/maintenance-reminders/maintenance-reminders').then(
            (m) => m.MaintenanceReminders,
          ),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
