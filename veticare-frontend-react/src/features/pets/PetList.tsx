import { useEffect, useState, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import Table from '@mui/material/Table';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import TablePagination from '@mui/material/TablePagination';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import TextField from '@mui/material/TextField';
import MenuItem from '@mui/material/MenuItem';
import Chip from '@mui/material/Chip';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import FolderOpenIcon from '@mui/icons-material/FolderOpen';
import { petService } from '../../core/services/pet.service';
import { ownerService } from '../../core/services/owner.service';
import { breedService } from '../../core/services/breed.service';
import type { Pet, PetGender, PetRequest } from '../../core/models/pet.model';
import type { Owner } from '../../core/models/owner.model';
import type { Breed } from '../../core/models/breed.model';
import { useNotification } from '../../core/context/NotificationContext';
import PageHeader from '../../shared/components/PageHeader';
import ConfirmDialog from '../../shared/components/ConfirmDialog';
import PetFormDialog from './PetFormDialog';

export default function PetList() {
  const navigate = useNavigate();
  const { success, error } = useNotification();
  const [allPets, setAllPets]   = useState<Pet[]>([]);
  const [owners, setOwners]     = useState<Owner[]>([]);
  const [breeds, setBreeds]     = useState<Breed[]>([]);
  const [loading, setLoading]   = useState(false);
  const [formOpen, setFormOpen] = useState(false);
  const [editing, setEditing]   = useState<Pet | undefined>();
  const [deleteTarget, setDeleteTarget] = useState<Pet | undefined>();
  const [page, setPage]         = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [search, setSearch]           = useState('');
  const [filterOwner, setFilterOwner] = useState<number | ''>('');
  const [filterBreed, setFilterBreed] = useState<number | ''>('');
  const [filterGender, setFilterGender] = useState<PetGender | ''>('');

  const load = async () => {
    setLoading(true);
    try { setAllPets((await petService.getAll()) ?? []); }
    catch { error('Error al cargar mascotas'); }
    finally { setLoading(false); }
  };

  useEffect(() => {
    load();
    ownerService.getAll().then(r => setOwners(r ?? []));
    breedService.getAll().then(r => setBreeds(r ?? []));
  }, []);

  const filtered = useMemo(() => allPets.filter(p => {
    const matchSearch = !search || p.name.toLowerCase().includes(search.toLowerCase());
    const matchOwner  = !filterOwner  || p.ownerId  === filterOwner;
    const matchBreed  = !filterBreed  || p.breedId  === filterBreed;
    const matchGender = !filterGender || p.gender   === filterGender;
    return matchSearch && matchOwner && matchBreed && matchGender;
  }), [allPets, search, filterOwner, filterBreed, filterGender]);

  const paged = filtered.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

  const handleSave = async (data: PetRequest) => {
    try {
      if (editing) { await petService.update(editing.id, data); success('Mascota actualizada'); }
      else         { await petService.create(data);             success('Mascota creada');     }
      setFormOpen(false); load();
    } catch { error('Error al guardar'); }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    try { await petService.delete(deleteTarget.id); success('Mascota eliminada'); load(); }
    catch { error('Error al eliminar'); }
    finally { setDeleteTarget(undefined); }
  };

  return (
    <Box>
      <PageHeader title="Mascotas" subtitle="Registro de pacientes"
        showAddButton addButtonText="Nueva Mascota"
        onAdd={() => { setEditing(undefined); setFormOpen(true); }} />

      {/* Filtros */}
      <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', mb: 2 }}>
        <TextField label="Buscar" size="small" value={search}
          onChange={e => { setSearch(e.target.value); setPage(0); }} sx={{ minWidth: 160 }} />
        <TextField label="Propietario" select size="small" value={filterOwner} sx={{ minWidth: 160 }}
          onChange={e => { setFilterOwner(e.target.value === '' ? '' : Number(e.target.value)); setPage(0); }}>
          <MenuItem value="">Todos</MenuItem>
          {owners.map(o => <MenuItem key={o.id} value={o.id}>{o.firstName} {o.lastName}</MenuItem>)}
        </TextField>
        <TextField label="Raza" select size="small" value={filterBreed} sx={{ minWidth: 140 }}
          onChange={e => { setFilterBreed(e.target.value === '' ? '' : Number(e.target.value)); setPage(0); }}>
          <MenuItem value="">Todas</MenuItem>
          {breeds.map(b => <MenuItem key={b.id} value={b.id}>{b.name}</MenuItem>)}
        </TextField>
        <TextField label="Género" select size="small" value={filterGender} sx={{ minWidth: 120 }}
          onChange={e => { setFilterGender(e.target.value as PetGender | ''); setPage(0); }}>
          <MenuItem value="">Todos</MenuItem>
          <MenuItem value="Male">♂ Macho</MenuItem>
          <MenuItem value="Female">♀ Hembra</MenuItem>
        </TextField>
      </Box>

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', pt: 6 }}><CircularProgress /></Box>
      ) : (
        <Paper>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Nombre</TableCell>
                <TableCell>Género</TableCell>
                <TableCell>Peso</TableCell>
                <TableCell>Propietario</TableCell>
                <TableCell>Raza</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {paged.length === 0 ? (
                <TableRow><TableCell colSpan={6}>
                  <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
                    No hay mascotas registradas.
                  </Typography>
                </TableCell></TableRow>
              ) : paged.map(p => (
                <TableRow key={p.id} hover>
                  <TableCell>{p.name}</TableCell>
                  <TableCell>
                    <Chip size="small"
                      label={p.gender === 'Male' ? '♂ Macho' : '♀ Hembra'}
                      sx={{
                        backgroundColor: p.gender === 'Male' ? '#e3f2fd' : '#fce4ec',
                        color: p.gender === 'Male' ? '#1565c0' : '#880e4f',
                        fontWeight: 500,
                      }} />
                  </TableCell>
                  <TableCell>{p.weight} kg</TableCell>
                  <TableCell>{p.owner ? `${p.owner.firstName} ${p.owner.lastName}` : '—'}</TableCell>
                  <TableCell>{p.breed?.name ?? '—'}</TableCell>
                  <TableCell>
                    <Tooltip title="Ver historial">
                      <IconButton color="secondary" onClick={() => navigate(`/pets/${p.id}`)}>
                        <FolderOpenIcon />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Editar">
                      <IconButton color="primary" onClick={() => { setEditing(p); setFormOpen(true); }}>
                        <EditIcon />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Eliminar">
                      <IconButton color="error" onClick={() => setDeleteTarget(p)}>
                        <DeleteIcon />
                      </IconButton>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          <TablePagination
            component="div"
            count={filtered.length}
            page={page}
            rowsPerPage={rowsPerPage}
            onPageChange={(_, p) => setPage(p)}
            onRowsPerPageChange={e => { setRowsPerPage(Number(e.target.value)); setPage(0); }}
            rowsPerPageOptions={[5, 10, 25]}
            labelRowsPerPage="Por página:"
          />
        </Paper>
      )}

      <PetFormDialog open={formOpen} pet={editing} owners={owners} breeds={breeds}
        onClose={() => setFormOpen(false)} onSave={handleSave} />

      <ConfirmDialog
        open={!!deleteTarget}
        title="Eliminar mascota"
        message={deleteTarget ? `¿Eliminar a ${deleteTarget.name}?` : ''}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(undefined)} />
    </Box>
  );
}
