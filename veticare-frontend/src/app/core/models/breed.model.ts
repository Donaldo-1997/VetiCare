export interface Breed {
  id: number;
  name: string;
  species: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface BreedRequest {
  name: string;
  species: string;
}
