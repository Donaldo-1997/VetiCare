import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import type { Vet, VetRequest } from '../../core/models/vet.model';

interface Props {
  open: boolean;
  vet?: Vet;
  onClose: () => void;
  onSave: (data: VetRequest) => void;
}

const empty: VetRequest = { firstName: '', lastName: '', licenseNumber: '', specialty: '' };

export default function VetFormDialog({ open, vet, onClose, onSave }: Props) {
  const [form, setForm] = useState<VetRequest>(empty);
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(vet ? {
      firstName: vet.firstName, lastName: vet.lastName,
      licenseNumber: vet.licenseNumber, specialty: vet.specialty,
    } : empty);
    setTouched({});
  }, [open, vet]);

  const set = (field: keyof VetRequest) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [field]: e.target.value }));
  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    firstName:     !form.firstName,
    lastName:      !form.lastName,
    licenseNumber: !form.licenseNumber,
    specialty:     !form.specialty,
  };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{vet ? 'Editar' : 'Nuevo'} Veterinario</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
          <TextField label="Nombre" fullWidth value={form.firstName} onChange={set('firstName')}
            onBlur={blur('firstName')} error={touched.firstName && errors.firstName}
            helperText={touched.firstName && errors.firstName ? 'Requerido' : ''} />
          <TextField label="Apellido" fullWidth value={form.lastName} onChange={set('lastName')}
            onBlur={blur('lastName')} error={touched.lastName && errors.lastName}
            helperText={touched.lastName && errors.lastName ? 'Requerido' : ''} />
        </Box>
        <TextField label="Número de licencia" fullWidth value={form.licenseNumber}
          onChange={set('licenseNumber')} onBlur={blur('licenseNumber')} sx={{ mt: 2 }}
          error={touched.licenseNumber && errors.licenseNumber}
          helperText={touched.licenseNumber && errors.licenseNumber ? 'Requerido' : ''} />
        <TextField label="Especialidad" fullWidth value={form.specialty}
          onChange={set('specialty')} onBlur={blur('specialty')} sx={{ mt: 2 }}
          error={touched.specialty && errors.specialty}
          helperText={touched.specialty && errors.specialty ? 'Requerido' : ''} />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid} onClick={() => onSave(form)}>
          {vet ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
