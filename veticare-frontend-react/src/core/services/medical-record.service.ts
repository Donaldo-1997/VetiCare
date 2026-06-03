import { api } from './api.service';
import type { MedicalRecord, MedicalRecordRequest } from '../models/medical-record.model';

const ENDPOINT = 'MedicalRecord';

export const medicalRecordService = {
  getAll:    ()                                      => api.get<MedicalRecord[]>(ENDPOINT),
  getById:   (id: number)                            => api.get<MedicalRecord>(`${ENDPOINT}/${id}`),
  getByPet:  (petId: number)                         => api.get<MedicalRecord[]>(`${ENDPOINT}/pet/${petId}`),
  create:    (body: MedicalRecordRequest)             => api.post<MedicalRecord>(ENDPOINT, body),
  update:    (id: number, body: MedicalRecordRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  delete:    (id: number)                            => api.delete<void>(`${ENDPOINT}/${id}`),
};
