import { Prescription } from './prescription.model';

export interface MedicalRecord {
  id: number;
  diagnosis: string;
  treatment: string;
  notes?: string;
  petId: number;
  appointmentId: number;
  prescriptions?: Prescription[];
  createdAt: string;
  updatedAt: string | null;
}

export interface MedicalRecordRequest {
  diagnosis: string;
  treatment: string;
  notes?: string;
  petId: number;
  appointmentId: number;
}
