import type { Breed } from './breed.model';
import type { Owner } from './owner.model';

export type PetGender = 'Male' | 'Female';

export interface Pet {
  id: number;
  name: string;
  birthDate: string;
  weight: number;
  gender: PetGender;
  ownerId: number;
  breedId: number;
  ownerName?: Owner;
  breedName?: Breed;
  createdAt: string;
  updatedAt: string | null;
}

export interface PetRequest {
  name: string;
  birthDate: string;
  weight: number;
  gender: PetGender;
  ownerId: number;
  breedId: number;
}
