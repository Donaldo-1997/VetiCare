import { api } from './api.service';
import type { Appointment, AppointmentRequest, AppointmentStatusRequest } from '../models/appointment.model';
import { AppointmentStatusToNumber, NumberToAppointmentStatus } from '../models/appointment.model';

const ENDPOINT = 'Appointments';

/** Convierte el status numérico que devuelve el backend al string que usa el frontend. */
function normalize(a: Appointment): Appointment {
  return {
    ...a,
    status: typeof a.status === 'number'
      ? NumberToAppointmentStatus[a.status as unknown as number]
      : a.status,
  };
}

export const appointmentService = {
  getAll:   ()           => api.get<Appointment[]>(ENDPOINT).then(r => r.map(normalize)),
  getById:  (id: number) => api.get<Appointment>(`${ENDPOINT}/${id}`).then(normalize),
  getByPet: (petId: number) => api.get<Appointment[]>(`${ENDPOINT}/pet/${petId}`).then(r => r.map(normalize)),
  create:   (body: AppointmentRequest) => api.post<Appointment>(ENDPOINT, body).then(normalize),
  update:   (id: number, body: AppointmentRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  /** Convierte el AppointmentStatus string al entero que espera el backend antes de enviarlo. */
  updateStatus: (id: number, body: AppointmentStatusRequest) =>
    api.patch<void>(`${ENDPOINT}/${id}/status`, {
      status: AppointmentStatusToNumber[body.status],
    }),
  delete: (id: number) => api.delete<void>(`${ENDPOINT}/${id}`),
};
