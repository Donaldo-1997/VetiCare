import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // ── Fase F2: CRUD de entidades base ─────────────────────────────────────────
  // { path: 'owners',    loadComponent: () => import('./features/owners/owners-page.component').then(m => m.OwnersPageComponent) },
  // { path: 'pets',      loadComponent: () => import('./features/pets/pets-page.component').then(m => m.PetsPageComponent) },
  // { path: 'vets',      loadComponent: () => import('./features/vets/vets-page.component').then(m => m.VetsPageComponent) },
  // { path: 'medicines', loadComponent: () => import('./features/medicines/medicines-page.component').then(m => m.MedicinesPageComponent) },

  // ── Fase F3: Citas ───────────────────────────────────────────────────────────
  // { path: 'appointments',     loadComponent: () => import('./features/appointments/appointments-page.component').then(m => m.AppointmentsPageComponent) },
  // { path: 'appointments/:id', loadComponent: () => import('./features/appointments/appointment-detail.component').then(m => m.AppointmentDetailComponent) },

  // ── Fase F4: Historial médico ────────────────────────────────────────────────
  // { path: 'medical-records/:id', loadComponent: () => import('./features/medical-records/medical-record-detail.component').then(m => m.MedicalRecordDetailComponent) },

  // ── Fase F5: Dashboard ───────────────────────────────────────────────────────
  // { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },

  // ── Fase F6: Auth (protegido con guards) ─────────────────────────────────────
  // { path: 'login', loadComponent: () => import('./features/auth/login.component').then(m => m.LoginComponent) },

  { path: '**', redirectTo: 'dashboard' },
];
