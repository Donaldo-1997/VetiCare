export interface Vet {
  id: number;
  firstName: string;
  lastName: string;
  licenseNumber: string;
  specialty: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface VetRequest {
  firstName: string;
  lastName: string;
  licenseNumber: string;
  specialty: string;
}
