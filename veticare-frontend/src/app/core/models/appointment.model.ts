import { Pet } from './pet.model';
import { Vet } from './vet.model';

export type AppointmentStatus = 'Scheduled' | 'InProgress' | 'Completed' | 'Cancelled';

export interface Appointment {
  id: number;
  scheduledAt: string;
  status: AppointmentStatus;
  reason: string;
  petId: number;
  vetId: number;
  pet?: Pet;
  vet?: Vet;
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
  status: AppointmentStatus;
}

// Configuración visual por estado
export const APPOINTMENT_STATUS_CONFIG: Record<AppointmentStatus, { label: string; color: string; icon: string }> = {
  Scheduled:  { label: 'Agendada',   color: 'accent',  icon: 'schedule'     },
  InProgress: { label: 'En curso',   color: 'primary', icon: 'play_circle'  },
  Completed:  { label: 'Completada', color: 'primary', icon: 'check_circle' },
  Cancelled:  { label: 'Cancelada',  color: 'warn',    icon: 'cancel'       },
};

// Transiciones válidas (máquina de estados)
export const APPOINTMENT_STATUS_TRANSITIONS: Record<AppointmentStatus, AppointmentStatus[]> = {
  Scheduled:  ['InProgress', 'Cancelled'],
  InProgress: ['Completed',  'Cancelled'],
  Completed:  [],
  Cancelled:  [],
};
