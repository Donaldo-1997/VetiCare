import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import type { MedicalRecord, MedicalRecordRequest } from '../../core/models/medical-record.model';
import type { Appointment } from '../../core/models/appointment.model';

interface Props {
  open: boolean;
  record?: MedicalRecord;
  petId: number;
  appointments: Appointment[];
  onClose: () => void;
  onSave: (data: MedicalRecordRequest) => void;
}

export default function MedicalRecordFormDialog({ open, record, petId, appointments, onClose, onSave }: Props) {
  const [form, setForm] = useState<MedicalRecordRequest>({
    diagnosis: '', treatment: '', notes: '', petId, appointmentId: 0,
  });
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(record ? {
      diagnosis: record.diagnosis, treatment: record.treatment,
      notes: record.notes ?? '', petId, appointmentId: record.appointmentId,
    } : { diagnosis: '', treatment: '', notes: '', petId, appointmentId: 0 });
    setTouched({});
  }, [open, record, petId]);

  const set = (field: keyof MedicalRecordRequest) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [field]: e.target.value }));
  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    diagnosis:     !form.diagnosis,
    treatment:     !form.treatment,
    appointmentId: !form.appointmentId,
  };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{record ? 'Editar' : 'Nuevo'} Registro Médico</DialogTitle>
      <DialogContent>
        <TextField label="Cita asociada" select fullWidth value={form.appointmentId || ''} sx={{ mt: 1 }}
          onChange={e => setForm(f => ({ ...f, appointmentId: Number(e.target.value) }))}
          onBlur={blur('appointmentId')} error={touched.appointmentId && errors.appointmentId}
          helperText={touched.appointmentId && errors.appointmentId ? 'Requerido' : ''}>
          {appointments.map(a => (
            <MenuItem key={a.id} value={a.id}>
              #{a.id} — {new Date(a.scheduledAt).toLocaleDateString('es-CO')} — {a.reason}
            </MenuItem>
          ))}
        </TextField>
        <TextField label="Diagnóstico" fullWidth multiline rows={2} value={form.diagnosis}
          onChange={set('diagnosis')} onBlur={blur('diagnosis')} sx={{ mt: 2 }}
          error={touched.diagnosis && errors.diagnosis}
          helperText={touched.diagnosis && errors.diagnosis ? 'Requerido' : ''} />
        <TextField label="Tratamiento" fullWidth multiline rows={2} value={form.treatment}
          onChange={set('treatment')} onBlur={blur('treatment')} sx={{ mt: 2 }}
          error={touched.treatment && errors.treatment}
          helperText={touched.treatment && errors.treatment ? 'Requerido' : ''} />
        <TextField label="Notas (opcional)" fullWidth multiline rows={2} value={form.notes}
          onChange={set('notes')} sx={{ mt: 2 }} />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid} onClick={() => onSave(form)}>
          {record ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
