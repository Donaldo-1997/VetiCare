import { api } from './api.service';
import type { Appointment, AppointmentRequest, AppointmentStatusRequest } from '../models/appointment.model';

const ENDPOINT = 'Appointments';

export const appointmentService = {
  getAll:       ()                                  => api.get<Appointment[]>(ENDPOINT),
  getById:      (id: number)                        => api.get<Appointment>(`${ENDPOINT}/${id}`),
  getByPet:     (petId: number)                     => api.get<Appointment[]>(`${ENDPOINT}/pet/${petId}`),
  create:       (body: AppointmentRequest)          => api.post<Appointment>(ENDPOINT, body),
  update:       (id: number, body: AppointmentRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  updateStatus: (id: number, body: AppointmentStatusRequest) => api.patch<void>(`${ENDPOINT}/${id}/status`, body),
  delete:       (id: number)                        => api.delete<void>(`${ENDPOINT}/${id}`),
};
