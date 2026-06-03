import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import type { Owner, OwnerRequest } from '../../core/models/owner.model';

interface Props {
  open: boolean;
  owner?: Owner;
  onClose: () => void;
  onSave: (data: OwnerRequest) => void;
}

const empty: OwnerRequest = { firstName: '', lastName: '', email: '', phone: '', address: '' };

export default function OwnerFormDialog({ open, owner, onClose, onSave }: Props) {
  const [form, setForm] = useState<OwnerRequest>(empty);
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(owner ? {
      firstName: owner.firstName, lastName: owner.lastName,
      email: owner.email, phone: owner.phone, address: owner.address,
    } : empty);
    setTouched({});
  }, [open, owner]);

  const set = (field: keyof OwnerRequest) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [field]: e.target.value }));
  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    firstName: !form.firstName,
    lastName:  !form.lastName,
    email:     !form.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email),
    phone:     !form.phone,
  };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{owner ? 'Editar' : 'Nuevo'} Propietario</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
          <TextField label="Nombre" fullWidth value={form.firstName} onChange={set('firstName')}
            onBlur={blur('firstName')} error={touched.firstName && errors.firstName}
            helperText={touched.firstName && errors.firstName ? 'Requerido' : ''} />
          <TextField label="Apellido" fullWidth value={form.lastName} onChange={set('lastName')}
            onBlur={blur('lastName')} error={touched.lastName && errors.lastName}
            helperText={touched.lastName && errors.lastName ? 'Requerido' : ''} />
        </Box>
        <TextField label="Email" fullWidth type="email" value={form.email} onChange={set('email')}
          onBlur={blur('email')} error={touched.email && errors.email} sx={{ mt: 2 }}
          helperText={touched.email && errors.email ? 'Email inválido' : ''} />
        <TextField label="Teléfono" fullWidth value={form.phone} onChange={set('phone')}
          onBlur={blur('phone')} error={touched.phone && errors.phone} sx={{ mt: 2 }}
          helperText={touched.phone && errors.phone ? 'Requerido' : ''} />
        <TextField label="Dirección" fullWidth value={form.address} onChange={set('address')} sx={{ mt: 2 }} />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid} onClick={() => onSave(form)}>
          {owner ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
