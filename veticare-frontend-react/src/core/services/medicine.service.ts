import { api } from './api.service';
import type { Medicine, MedicineRequest } from '../models/medicine.model';

const ENDPOINT = 'Medicine';

export const medicineService = {
  getAll:  ()                              => api.get<Medicine[]>(ENDPOINT),
  getById: (id: number)                    => api.get<Medicine>(`${ENDPOINT}/${id}`),
  create:  (body: MedicineRequest)         => api.post<Medicine>(ENDPOINT, body),
  update:  (id: number, body: MedicineRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  delete:  (id: number)                    => api.delete<void>(`${ENDPOINT}/${id}`),
};
