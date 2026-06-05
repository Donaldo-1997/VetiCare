import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import { isValid, parseISO, format } from 'date-fns';
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

const emptyForm = { reason: '', petId: 0, vetId: 0 };

export default function AppointmentFormDialog({ open, appointment, pets, vets, onClose, onSave }: Props) {
  const [form, setForm]         = useState(emptyForm);
  const [scheduledAt, setScheduledAt] = useState<Date | null>(null);
  const [touched, setTouched]   = useState<Record<string, boolean>>({});

  useEffect(() => {
    if (appointment) {
      setForm({ reason: appointment.reason, petId: appointment.petId, vetId: appointment.vetId });
      const parsed = parseISO(appointment.scheduledAt);
      setScheduledAt(isValid(parsed) ? parsed : null);
    } else {
      setForm(emptyForm);
      setScheduledAt(null);
    }
    setTouched({});
  }, [open, appointment]);

  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    scheduledAt: !scheduledAt || !isValid(scheduledAt),
    reason:      !form.reason,
    petId:       !form.petId,
    vetId:       !form.vetId,
  };
  const isFormValid = !Object.values(errors).some(Boolean);

  const handleSave = () => {
    // Convierte Date a 'yyyy-MM-dd'T'HH:mm:ss' para el backend
    onSave({
      ...form,
      scheduledAt: scheduledAt ? format(scheduledAt, "yyyy-MM-dd'T'HH:mm:ss") : '',
    });
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{appointment ? 'Editar' : 'Nueva'} Cita</DialogTitle>
      <DialogContent>
        <DateTimePicker
          label="Fecha y hora"
          value={scheduledAt}
          onChange={date => { setScheduledAt(date); setTouched(t => ({ ...t, scheduledAt: true })); }}
          format="dd/MM/yyyy HH:mm"
          ampm={false}
          slotProps={{
            textField: {
              fullWidth: true,
              sx: { mt: 1 },
              error: touched.scheduledAt && errors.scheduledAt,
              helperText: touched.scheduledAt && errors.scheduledAt ? 'Requerido' : '',
              onBlur: blur('scheduledAt'),
            },
          }}
        />

        <TextField
          label="Motivo" fullWidth multiline rows={2} value={form.reason}
          onChange={e => setForm(f => ({ ...f, reason: e.target.value }))}
          onBlur={blur('reason')} sx={{ mt: 2 }}
          error={touched.reason && errors.reason}
          helperText={touched.reason && errors.reason ? 'Requerido' : ''} />

        <TextField
          label="Mascota" select fullWidth value={form.petId || ''} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, petId: Number(e.target.value) }))}
          onBlur={blur('petId')} error={touched.petId && errors.petId}
          helperText={touched.petId && errors.petId ? 'Requerido' : ''}>
          {pets.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
        </TextField>

        <TextField
          label="Veterinario" select fullWidth value={form.vetId || ''} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, vetId: Number(e.target.value) }))}
          onBlur={blur('vetId')} error={touched.vetId && errors.vetId}
          helperText={touched.vetId && errors.vetId ? 'Requerido' : ''}>
          {vets.map(v => <MenuItem key={v.id} value={v.id}>Dr. {v.firstName} {v.lastName}</MenuItem>)}
        </TextField>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isFormValid} onClick={handleSave}>
          {appointment ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
