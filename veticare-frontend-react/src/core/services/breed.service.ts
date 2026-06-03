import { api } from './api.service';
import type { Breed } from '../models/breed.model';

const ENDPOINT = 'Breed';

export const breedService = {
  getAll:  () => api.get<Breed[]>(ENDPOINT),
  getById: (id: number) => api.get<Breed>(`${ENDPOINT}/${id}`),
};
