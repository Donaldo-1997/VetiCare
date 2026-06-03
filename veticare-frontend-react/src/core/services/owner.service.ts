import { api } from './api.service';
import type { Owner, OwnerRequest } from '../models/owner.model';

const ENDPOINT = 'Owner';

export const ownerService = {
  getAll:   ()                          => api.get<Owner[]>(ENDPOINT),
  getById:  (id: number)                => api.get<Owner>(`${ENDPOINT}/${id}`),
  create:   (body: OwnerRequest)        => api.post<Owner>(ENDPOINT, body),
  update:   (id: number, body: OwnerRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  delete:   (id: number)                => api.delete<void>(`${ENDPOINT}/${id}`),
};
