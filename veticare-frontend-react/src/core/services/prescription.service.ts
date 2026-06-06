import { api } from './api.service';
import type { Prescription, PrescriptionRequest } from '../models/prescription.model';

const ENDPOINT = 'Prescription';

export const prescriptionService = {
  getAll:           ()                                    => api.get<Prescription[]>(ENDPOINT),
  getById:          (id: number)                          => api.get<Prescription>(`${ENDPOINT}/${id}`),
  getByMedicalRecord: (medicalRecordId: number)           => api.get<Prescription[]>(`${ENDPOINT}/medicalrecord/${medicalRecordId}`),
  create:           (body: PrescriptionRequest)           => api.post<Prescription>(ENDPOINT, body),
  update:           (id: number, body: PrescriptionRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  delete:           (id: number)                          => api.delete<void>(`${ENDPOINT}/${id}`),
};
