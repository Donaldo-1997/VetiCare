import { useEffect, useState } from 'react';
import Table from '@mui/material/Table';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Chip from '@mui/material/Chip';
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { medicineService } from '../../core/services/medicine.service';
import type { Medicine, MedicineRequest } from '../../core/models/medicine.model';
import { useNotification } from '../../core/context/NotificationContext';
import PageHeader from '../../shared/components/PageHeader';
import ConfirmDialog from '../../shared/components/ConfirmDialog';
import MedicineFormDialog from './MedicineFormDialog';

export default function MedicineList() {
  const { success, error } = useNotification();
  const [medicines, setMedicines] = useState<Medicine[]>([]);
  const [loading, setLoading]     = useState(false);
  const [formOpen, setFormOpen]   = useState(false);
  const [editing, setEditing]     = useState<Medicine | undefined>();
  const [deleteTarget, setDeleteTarget] = useState<Medicine | undefined>();

  const load = async () => {
    setLoading(true);
    try { setMedicines((await medicineService.getAll()) ?? []); }
    catch { error('Error al cargar medicamentos'); }
    finally { setLoading(false); }
  };

  useEffect(() => { load(); }, []);

  const handleSave = async (data: MedicineRequest) => {
    try {
      if (editing) { await medicineService.update(editing.id, data); success('Medicamento actualizado'); }
      else         { await medicineService.create(data);             success('Medicamento creado');     }
      setFormOpen(false); load();
    } catch { error('Error al guardar'); }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    try { await medicineService.delete(deleteTarget.id); success('Medicamento eliminado'); load(); }
    catch { error('Error al eliminar'); }
    finally { setDeleteTarget(undefined); }
  };

  return (
    <Box>
      <PageHeader title="Medicamentos" subtitle="Catálogo de medicamentos disponibles"
        showAddButton addButtonText="Nuevo Medicamento" onAdd={() => { setEditing(undefined); setFormOpen(true); }} />

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', pt: 6 }}><CircularProgress /></Box>
      ) : (
        <Paper>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Nombre</TableCell>
                <TableCell>Principio activo</TableCell>
                <TableCell>Unidad</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {medicines.length === 0 ? (
                <TableRow><TableCell colSpan={4}>
                  <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
                    No hay medicamentos registrados.
                  </Typography>
                </TableCell></TableRow>
              ) : medicines.map(m => (
                <TableRow key={m.id} hover>
                  <TableCell>{m.name}</TableCell>
                  <TableCell>{m.activeIngredient}</TableCell>
                  <TableCell><Chip label={m.unit} size="small" /></TableCell>
                  <TableCell>
                    <Tooltip title="Editar">
                      <IconButton color="primary" onClick={() => { setEditing(m); setFormOpen(true); }}>
                        <EditIcon />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Eliminar">
                      <IconButton color="error" onClick={() => setDeleteTarget(m)}><DeleteIcon /></IconButton>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </Paper>
      )}

      <MedicineFormDialog open={formOpen} medicine={editing}
        onClose={() => setFormOpen(false)} onSave={handleSave} />

      <ConfirmDialog
        open={!!deleteTarget}
        title="Eliminar medicamento"
        message={deleteTarget ? `¿Eliminar "${deleteTarget.name}"?` : ''}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(undefined)} />
    </Box>
  );
}
