export interface Owner {
  id: number;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  address: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface OwnerRequest {
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  address: string;
}
