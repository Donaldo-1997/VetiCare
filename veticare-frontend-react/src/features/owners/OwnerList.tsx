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
import { ownerService } from '../../core/services/owner.service';
import type { Owner, OwnerRequest } from '../../core/models/owner.model';
import { useNotification } from '../../core/context/NotificationContext';
import PageHeader from '../../shared/components/PageHeader';
import ConfirmDialog from '../../shared/components/ConfirmDialog';
import OwnerFormDialog from './OwnerFormDialog';
import { parseApiError } from '../../core/utils/error.utils';

export default function OwnerList() {
  const { success, error } = useNotification();
  const [owners, setOwners]       = useState<Owner[]>([]);
  const [loading, setLoading]     = useState(false);
  const [formOpen, setFormOpen]   = useState(false);
  const [editing, setEditing]     = useState<Owner | undefined>();
  const [deleteTarget, setDeleteTarget] = useState<Owner | undefined>();

  const load = async () => {
    setLoading(true);
    try { setOwners((await ownerService.getAll()) ?? []); }
    catch (err) { error(parseApiError(err, 'Error al cargar propietarios')); }
    finally { setLoading(false); }
  };

  useEffect(() => { load(); }, []);

  const openForm = (owner?: Owner) => { setEditing(owner); setFormOpen(true); };

  const handleSave = async (data: OwnerRequest) => {
    try {
      if (editing) { await ownerService.update(editing.id, data); success('Propietario actualizado'); }
      else         { await ownerService.create(data);              success('Propietario creado');     }
      setFormOpen(false); load();
    } catch (err) { error(parseApiError(err, 'Error al guardar propietario')); }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    try { await ownerService.delete(deleteTarget.id); success('Propietario eliminado'); load(); }
    catch (err) { error(parseApiError(err, 'Error al eliminar propietario')); }
    finally { setDeleteTarget(undefined); }
  };

  return (
    <Box>
      <PageHeader title="Propietarios" subtitle="Gestión de dueños de mascotas"
        showAddButton addButtonText="Nuevo Propietario" onAdd={() => openForm()} />

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', pt: 6 }}><CircularProgress /></Box>
      ) : (
        <Paper>
          <TableContainer>
          <Table sx={{ minWidth: 600 }}>
            <TableHead>
              <TableRow>
                <TableCell>Nombre</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Teléfono</TableCell>
                <TableCell>Dirección</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {owners.length === 0 ? (
                <TableRow><TableCell colSpan={5}>
                  <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
                    No hay propietarios registrados.
                  </Typography>
                </TableCell></TableRow>
              ) : owners.map(o => (
                <TableRow key={o.id} hover>
                  <TableCell>{o.firstName} {o.lastName}</TableCell>
                  <TableCell>{o.email}</TableCell>
                  <TableCell>{o.phone}</TableCell>
                  <TableCell>{o.address}</TableCell>
                  <TableCell>
                    <Tooltip title="Editar">
                      <IconButton color="primary" onClick={() => openForm(o)}><EditIcon /></IconButton>
                    </Tooltip>
                    <Tooltip title="Eliminar">
                      <IconButton color="error" onClick={() => setDeleteTarget(o)}><DeleteIcon /></IconButton>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          </TableContainer>
        </Paper>
      )}

      <OwnerFormDialog open={formOpen} owner={editing}
        onClose={() => setFormOpen(false)} onSave={handleSave} />

      <ConfirmDialog
        open={!!deleteTarget}
        title="Eliminar propietario"
        message={deleteTarget ? `¿Eliminar a ${deleteTarget.firstName} ${deleteTarget.lastName}?` : ''}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(undefined)} />
    </Box>
  );
}
