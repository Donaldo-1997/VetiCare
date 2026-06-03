import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import type { Appointment, AppointmentRequest } from '../../core/models/appointment.model';
import type { Pet } from '../../core/models/pet.model';
import type { Vet } from '../../core/models/vet.model';

interface Props {
  open: boolean;
  appointment?: Appointment;
  pets: Pet[];
  vets: Vet[];
  onClose: () => void;
  onSave: (data: AppointmentRequest) => void;
}

const empty: AppointmentRequest = { scheduledAt: '', reason: '', petId: 0, vetId: 0 };

export default function AppointmentFormDialog({ open, appointment, pets, vets, onClose, onSave }: Props) {
  const [form, setForm] = useState<AppointmentRequest>(empty);
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(appointment ? {
      scheduledAt: appointment.scheduledAt.slice(0, 16),
      reason: appointment.reason,
      petId: appointment.petId,
      vetId: appointment.vetId,
    } : empty);
    setTouched({});
  }, [open, appointment]);

  const set = (field: keyof AppointmentRequest) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [field]: e.target.value }));
  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    scheduledAt: !form.scheduledAt,
    reason:      !form.reason,
    petId:       !form.petId,
    vetId:       !form.vetId,
  };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{appointment ? 'Editar' : 'Nueva'} Cita</DialogTitle>
      <DialogContent>
        <TextField label="Fecha y hora" type="datetime-local" fullWidth sx={{ mt: 1 }}
          slotProps={{ inputLabel: { shrink: true } }} value={form.scheduledAt}
          onChange={set('scheduledAt')} onBlur={blur('scheduledAt')}
          error={touched.scheduledAt && errors.scheduledAt}
          helperText={touched.scheduledAt && errors.scheduledAt ? 'Requerido' : ''} />

        <TextField label="Motivo" fullWidth multiline rows={2} value={form.reason}
          onChange={set('reason')} onBlur={blur('reason')} sx={{ mt: 2 }}
          error={touched.reason && errors.reason}
          helperText={touched.reason && errors.reason ? 'Requerido' : ''} />

        <TextField label="Mascota" select fullWidth value={form.petId || ''} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, petId: Number(e.target.value) }))}
          onBlur={blur('petId')} error={touched.petId && errors.petId}
          helperText={touched.petId && errors.petId ? 'Requerido' : ''}>
          {pets.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
        </TextField>

        <TextField label="Veterinario" select fullWidth value={form.vetId || ''} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, vetId: Number(e.target.value) }))}
          onBlur={blur('vetId')} error={touched.vetId && errors.vetId}
          helperText={touched.vetId && errors.vetId ? 'Requerido' : ''}>
          {vets.map(v => <MenuItem key={v.id} value={v.id}>Dr. {v.firstName} {v.lastName}</MenuItem>)}
        </TextField>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid} onClick={() => onSave(form)}>
          {appointment ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
