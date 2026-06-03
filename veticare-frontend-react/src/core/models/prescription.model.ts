import type { Medicine } from './medicine.model';

export interface Prescription {
  id: number;
  dosage: string;
  quantity: number;
  instructions: string;
  medicalRecordId: number;
  medicineId: number;
  medicine?: Medicine;
}

export interface PrescriptionRequest {
  dosage: string;
  quantity: number;
  instructions: string;
  medicalRecordId: number;
  medicineId: number;
}
