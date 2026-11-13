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
        path: 'dashboard',
        title: 'Dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: 'vehicles',
        title: 'Vehicles',
        loadComponent: () => import('./features/vehicles/vehicles').then((m) => m.Vehicles),
      },
      {
        path: 'vehicles/:vehicleId/maintenance-records',
        title: 'Maintenance',
        loadComponent: () =>
          import('./features/maintenance-records/maintenance-records').then(
            (m) => m.MaintenanceRecords,
          ),
      },
      {
        path: 'vehicles/:vehicleId/expenses',
        title: 'Expenses',
        loadComponent: () => import('./features/expenses/expenses').then((m) => m.Expenses),
      },
      {
        path: 'vehicles/:vehicleId/maintenance-reminders',
        title: 'Reminders',
        loadComponent: () =>
          import('./features/maintenance-reminders/maintenance-reminders').then(
            (m) => m.MaintenanceReminders,
          ),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
