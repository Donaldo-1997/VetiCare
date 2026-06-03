import { api } from './api.service';
import type { Pet, PetRequest } from '../models/pet.model';

const ENDPOINT = 'Pet';

export const petService = {
  getAll:      ()                        => api.get<Pet[]>(ENDPOINT),
  getById:     (id: number)              => api.get<Pet>(`${ENDPOINT}/${id}`),
  getByOwner:  (ownerId: number)         => api.get<Pet[]>(`${ENDPOINT}/owner/${ownerId}`),
  create:      (body: PetRequest)        => api.post<Pet>(ENDPOINT, body),
  update:      (id: number, body: PetRequest) => api.put<void>(`${ENDPOINT}/${id}`, body),
  delete:      (id: number)              => api.delete<void>(`${ENDPOINT}/${id}`),
};
