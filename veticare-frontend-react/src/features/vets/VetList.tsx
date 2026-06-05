import { useEffect, useState } from 'react';
import Table from '@mui/material/Table';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { vetService } from '../../core/services/vet.service';
import type { Vet, VetRequest } from '../../core/models/vet.model';
import { useNotification } from '../../core/context/NotificationContext';
import PageHeader from '../../shared/components/PageHeader';
import ConfirmDialog from '../../shared/components/ConfirmDialog';
import VetFormDialog from './VetFormDialog';
import { parseApiError } from '../../core/utils/error.utils';

export default function VetList() {
  const { success, error } = useNotification();
  const [vets, setVets]           = useState<Vet[]>([]);
  const [loading, setLoading]     = useState(false);
  const [formOpen, setFormOpen]   = useState(false);
  const [editing, setEditing]     = useState<Vet | undefined>();
  const [deleteTarget, setDeleteTarget] = useState<Vet | undefined>();

  const load = async () => {
    setLoading(true);
    try { setVets((await vetService.getAll()) ?? []); }
    catch (err) { error(parseApiError(err, 'Error al cargar veterinarios')); }
    finally { setLoading(false); }
  };

  useEffect(() => { load(); }, []);

  const openForm = (vet?: Vet) => { setEditing(vet); setFormOpen(true); };

  const handleSave = async (data: VetRequest) => {
    try {
      if (editing) { await vetService.update(editing.id, data); success('Veterinario actualizado'); }
      else         { await vetService.create(data);             success('Veterinario creado');     }
      setFormOpen(false); load();
    } catch (err) { error(parseApiError(err, 'Error al guardar veterinario')); }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    try { await vetService.delete(deleteTarget.id); success('Veterinario eliminado'); load(); }
    catch (err) { error(parseApiError(err, 'Error al eliminar veterinario')); }
    finally { setDeleteTarget(undefined); }
  };

  return (
    <Box>
      <PageHeader title="Veterinarios" subtitle="Gestión del personal médico"
        showAddButton addButtonText="Nuevo Veterinario" onAdd={() => openForm()} />

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', pt: 6 }}><CircularProgress /></Box>
      ) : (
        <Paper>
          <TableContainer>
          <Table sx={{ minWidth: 500 }}>
            <TableHead>
              <TableRow>
                <TableCell>Nombre</TableCell>
                <TableCell>Licencia</TableCell>
                <TableCell>Especialidad</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {vets.length === 0 ? (
                <TableRow><TableCell colSpan={4}>
                  <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
                    No hay veterinarios registrados.
                  </Typography>
                </TableCell></TableRow>
              ) : vets.map(v => (
                <TableRow key={v.id} hover>
                  <TableCell>{v.firstName} {v.lastName}</TableCell>
                  <TableCell>{v.licenseNumber}</TableCell>
                  <TableCell>{v.specialty}</TableCell>
                  <TableCell>
                    <Tooltip title="Editar">
                      <IconButton color="primary" onClick={() => openForm(v)}><EditIcon /></IconButton>
                    </Tooltip>
                    <Tooltip title="Eliminar">
                      <IconButton color="error" onClick={() => setDeleteTarget(v)}><DeleteIcon /></IconButton>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          </TableContainer>
        </Paper>
      )}

      <VetFormDialog open={formOpen} vet={editing}
        onClose={() => setFormOpen(false)} onSave={handleSave} />

      <ConfirmDialog
        open={!!deleteTarget}
        title="Eliminar veterinario"
        message={deleteTarget ? `¿Eliminar a ${deleteTarget.firstName} ${deleteTarget.lastName}?` : ''}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(undefined)} />
    </Box>
  );
}
