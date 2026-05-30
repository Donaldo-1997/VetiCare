export interface Medicine {
  id: number;
  name: string;
  activeIngredient: string;
  unit: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface MedicineRequest {
  name: string;
  activeIngredient: string;
  unit: string;
}
