import { Medicine } from './medicine.model';

export interface Prescription {
  id: number;
  quantity: number;
  dosage: string;
  instructions: string;
  medicalRecordId: number;
  medicineId: number;
  medicine?: Medicine;
  createdAt: string;
  updatedAt: string | null;
}

export interface PrescriptionRequest {
  quantity: number;
  dosage: string;
  instructions: string;
  medicalRecordId: number;
  medicineId: number;
}
