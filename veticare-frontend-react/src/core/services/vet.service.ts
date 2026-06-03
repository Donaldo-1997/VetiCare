import { api } from './api.service';
import type { Vet, VetRequest } from '../models/vet.model';

const ENDPOINT = 'Vets';

export const vetService = {
  getAll:  ()                        => api.get<Vet[]>(ENDPOINT),
  getById: (id: number)              => api.get<Vet>(`${ENDPOINT}/${id}`),
  create:  (body: VetRequest)        => api.post<Vet>(ENDPOINT, body),
  update:  (id: number, body: VetRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  delete:  (id: number)              => api.delete<void>(`${ENDPOINT}/${id}`),
};
