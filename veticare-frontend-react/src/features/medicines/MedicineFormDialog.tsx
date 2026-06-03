import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import type { Medicine, MedicineRequest } from '../../core/models/medicine.model';

interface Props {
  open: boolean;
  medicine?: Medicine;
  onClose: () => void;
  onSave: (data: MedicineRequest) => void;
}

const empty: MedicineRequest = { name: '', activeIngredient: '', unit: '' };

export default function MedicineFormDialog({ open, medicine, onClose, onSave }: Props) {
  const [form, setForm] = useState<MedicineRequest>(empty);
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(medicine ? { name: medicine.name, activeIngredient: medicine.activeIngredient, unit: medicine.unit } : empty);
    setTouched({});
  }, [open, medicine]);

  const set = (field: keyof MedicineRequest) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [field]: e.target.value }));
  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = { name: !form.name, activeIngredient: !form.activeIngredient, unit: !form.unit };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="xs" fullWidth>
      <DialogTitle>{medicine ? 'Editar' : 'Nuevo'} Medicamento</DialogTitle>
      <DialogContent>
        <TextField label="Nombre" fullWidth value={form.name} onChange={set('name')}
          onBlur={blur('name')} error={touched.name && errors.name}
          helperText={touched.name && errors.name ? 'Requerido' : ''} sx={{ mt: 1 }} />
        <TextField label="Principio activo" fullWidth value={form.activeIngredient}
          onChange={set('activeIngredient')} onBlur={blur('activeIngredient')} sx={{ mt: 2 }}
          error={touched.activeIngredient && errors.activeIngredient}
          helperText={touched.activeIngredient && errors.activeIngredient ? 'Requerido' : ''} />
        <TextField label="Unidad (ej: mg, ml)" fullWidth value={form.unit} onChange={set('unit')}
          onBlur={blur('unit')} error={touched.unit && errors.unit} sx={{ mt: 2 }}
          helperText={touched.unit && errors.unit ? 'Requerido' : ''} />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid} onClick={() => onSave(form)}>
          {medicine ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
