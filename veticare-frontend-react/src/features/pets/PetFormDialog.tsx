import { useEffect, useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import Box from '@mui/material/Box';
import type { Pet, PetGender, PetRequest } from '../../core/models/pet.model';
import type { Owner } from '../../core/models/owner.model';
import type { Breed } from '../../core/models/breed.model';

interface Props {
  open: boolean;
  pet?: Pet;
  owners: Owner[];
  breeds: Breed[];
  onClose: () => void;
  onSave: (data: PetRequest) => void;
}

const empty: PetRequest = { name: '', birthDate: '', weight: 0, gender: 'Male', ownerId: 0, breedId: 0 };

export default function PetFormDialog({ open, pet, owners, breeds, onClose, onSave }: Props) {
  const [form, setForm] = useState<PetRequest>(empty);
  const [touched, setTouched] = useState<Record<string, boolean>>({});

  useEffect(() => {
    setForm(pet ? {
      name: pet.name, birthDate: pet.birthDate.slice(0, 10),
      weight: pet.weight, gender: pet.gender, ownerId: pet.ownerId, breedId: pet.breedId,
    } : empty);
    setTouched({});
  }, [open, pet]);

  const set = (field: keyof PetRequest) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm(f => ({ ...f, [field]: e.target.value }));
  const blur = (field: string) => () => setTouched(t => ({ ...t, [field]: true }));

  const errors = {
    name:     !form.name,
    birthDate:!form.birthDate,
    weight:   !form.weight || form.weight <= 0,
    ownerId:  !form.ownerId,
    breedId:  !form.breedId,
  };
  const isValid = !Object.values(errors).some(Boolean);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{pet ? 'Editar' : 'Nueva'} Mascota</DialogTitle>
      <DialogContent>
        <TextField label="Nombre" fullWidth value={form.name} onChange={set('name')}
          onBlur={blur('name')} error={touched.name && errors.name}
          helperText={touched.name && errors.name ? 'Requerido' : ''} sx={{ mt: 1 }} />

        <Box sx={{ display: 'flex', gap: 2, mt: 2 }}>
          <TextField label="Fecha de nacimiento" type="date" fullWidth
            slotProps={{ inputLabel: { shrink: true } }} value={form.birthDate}
            onChange={set('birthDate')} onBlur={blur('birthDate')}
            error={touched.birthDate && errors.birthDate}
            helperText={touched.birthDate && errors.birthDate ? 'Requerido' : ''} />
          <TextField label="Peso (kg)" type="number" fullWidth value={form.weight || ''}
            onChange={set('weight')} onBlur={blur('weight')}
            error={touched.weight && errors.weight}
            helperText={touched.weight && errors.weight ? 'Debe ser mayor a 0' : ''} />
        </Box>

        <TextField label="Género" select fullWidth value={form.gender} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, gender: e.target.value as PetGender }))}>
          <MenuItem value="Male">♂ Macho</MenuItem>
          <MenuItem value="Female">♀ Hembra</MenuItem>
        </TextField>

        <TextField label="Propietario" select fullWidth value={form.ownerId || ''} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, ownerId: Number(e.target.value) }))}
          onBlur={blur('ownerId')} error={touched.ownerId && errors.ownerId}
          helperText={touched.ownerId && errors.ownerId ? 'Requerido' : ''}>
          {owners.map(o => (
            <MenuItem key={o.id} value={o.id}>{o.firstName} {o.lastName}</MenuItem>
          ))}
        </TextField>

        <TextField label="Raza" select fullWidth value={form.breedId || ''} sx={{ mt: 2 }}
          onChange={e => setForm(f => ({ ...f, breedId: Number(e.target.value) }))}
          onBlur={blur('breedId')} error={touched.breedId && errors.breedId}
          helperText={touched.breedId && errors.breedId ? 'Requerido' : ''}>
          {breeds.map(b => (
            <MenuItem key={b.id} value={b.id}>{b.name}</MenuItem>
          ))}
        </TextField>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancelar</Button>
        <Button variant="contained" disabled={!isValid}
          onClick={() => onSave({ ...form, weight: Number(form.weight) })}>
          {pet ? 'Actualizar' : 'Crear'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
