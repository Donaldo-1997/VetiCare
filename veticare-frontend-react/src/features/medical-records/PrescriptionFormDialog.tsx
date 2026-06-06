import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import Box from '@mui/material/Box';
import type { Prescription, PrescriptionRequest } from '../../core/models/prescription.model';
import type { Medicine } from '../../core/models/medicine.model';

interface Props {
  open: boolean;
  prescription?: Prescription;
  medicalRecordId: number;
  medicines: Medicine[];
  onClose: () => void;
  onSave: (data: PrescriptionRequest) => void;
}

const empty = (medicalRecordId: number): PrescriptionRequest => ({
  dosage: '',
  quantity: 1,
  instructions: '',
  medicalRecordId,
  medicineId: 0,
});

export default function PrescriptionFormDialog({
  open, prescription, medicalRecordId, medicines, onClose, onSave,
}: Props) {
  const [form, setForm] = useState<PrescriptionRequest>(empty(medicalRecordId));
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(prescription ? {
      dosage:          prescription.dosage,
      quantity:        prescription.quantity,
      instructions:    prescription.instructions,
      medicalRecordId,
      medicineId:      prescription.medicineId,
    } : empty(medicalRecordId));
    setTouched({});
  }, [open, prescription, medicalRecordId]);

  const set = (field: keyof PrescriptionRequest) =>
    (e: React.ChangeEvent<HTMLInputElement>) =>
      setForm(f => ({ ...f, [field]: field === 'quantity' ? Number(e.target.value) : e.target.value }));

  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    medicineId:   !form.medicineId,
    dosage:       !form.dosage,
    quantity:     !form.quantity || form.quantity <= 0,
    instructions: !form.instructions,
  };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{prescription ? 'Editar' : 'Nueva'} Prescripción</DialogTitle>
      <DialogContent>

        <TextField
          label="Medicamento" select fullWidth value={form.medicineId || ''} sx={{ mt: 1 }}
          onChange={e => setForm(f => ({ ...f, medicineId: Number(e.target.value) }))}
          onBlur={blur('medicineId')}
          error={touched.medicineId && errors.medicineId}
          helperText={touched.medicineId && errors.medicineId ? 'Requerido' : ''}>
          {medicines.map(m => (
            <MenuItem key={m.id} value={m.id}>
              {m.name} ({m.unit})
            </MenuItem>
          ))}
        </TextField>

        <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
          <TextField
            label="Dosis (ej: 250 mg)" fullWidth value={form.dosage}
            onChange={set('dosage')} onBlur={blur('dosage')}
            error={touched.dosage && errors.dosage}
            helperText={touched.dosage && errors.dosage ? 'Requerido' : ''} />
          <TextField
            label="Cantidad" type="number" sx={{ maxWidth: 120 }} value={form.quantity}
            onChange={set('quantity')} onBlur={blur('quantity')}
            inputProps={{ min: 1 }}
            error={touched.quantity && errors.quantity}
            helperText={touched.quantity && errors.quantity ? '> 0' : ''} />
        </Box>

        <TextField
          label="Instrucciones" fullWidth multiline rows={3} value={form.instructions}
          onChange={set('instructions')} onBlur={blur('instructions')} sx={{ mt: 2 }}
          placeholder="Ej: 1 tableta cada 12 horas durante 7 días, con alimento."
          error={touched.instructions && errors.instructions}
          helperText={touched.instructions && errors.instructions ? 'Requerido' : ''} />

      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid} onClick={() => onSave(form)}>
          {prescription ? 'Actualizar' : 'Agregar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
