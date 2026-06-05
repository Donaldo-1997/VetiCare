import type { Pet } from './pet.model';
import type { Vet } from './vet.model';

export type AppointmentStatus = 'Scheduled' | 'InProgress' | 'Completed' | 'Cancelled';

export interface Appointment {
  id: number;
  scheduledAt: string;
  status: AppointmentStatus;
  reason: string;
  petId: number;
  vetId: number;
  petName?: Pet;
  vetName?: Vet;
  createdAt: string;
  updatedAt: string | null;
}

export interface AppointmentRequest {
  scheduledAt: string;
  reason: string;
  petId: number;
  vetId: number;
}

export interface AppointmentStatusRequest {
  status: AppointmentStatus;  // el servicio convierte internamente a número antes de enviar
}

export const APPOINTMENT_STATUS_CONFIG: Record<AppointmentStatus, { label: string; color: string; icon: string }> = {
  Scheduled:  { label: 'Agendada',   color: '#0288d1', icon: 'schedule'      },
  InProgress: { label: 'En curso',   color: '#7b1fa2', icon: 'play_circle'   },
  Completed:  { label: 'Completada', color: '#388e3c', icon: 'check_circle'  },
  Cancelled:  { label: 'Cancelada',  color: '#d32f2f', icon: 'cancel'        },
};

export const APPOINTMENT_STATUS_TRANSITIONS: Record<AppointmentStatus, AppointmentStatus[]> = {
  Scheduled:  ['InProgress', 'Cancelled'],
  InProgress: ['Completed',  'Cancelled'],
  Completed:  [],
  Cancelled:  [],
};

export const AppointmentStatusToNumber: Record<AppointmentStatus, number> = {
  Scheduled:  0,
  InProgress: 1,
  Completed:  2,
  Cancelled:  3,
};

export const NumberToAppointmentStatus: Record<number, AppointmentStatus> = {
  0: 'Scheduled',
  1: 'InProgress',
  2: 'Completed',
  3: 'Cancelled',
};
