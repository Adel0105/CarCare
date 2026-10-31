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
    ],
  },
  { path: '**', redirectTo: '' },
];
