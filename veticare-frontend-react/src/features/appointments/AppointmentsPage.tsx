import { useEffect, useState, useMemo } from 'react';
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
import Menu from '@mui/material/Menu';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SwapHorizIcon from '@mui/icons-material/SwapHoriz';
import { appointmentService } from '../../core/services/appointment.service';
import { petService } from '../../core/services/pet.service';
import { vetService } from '../../core/services/vet.service';
import type{
  Appointment, AppointmentRequest, AppointmentStatus,
} from '../../core/models/appointment.model';
import { 
  APPOINTMENT_STATUS_CONFIG, APPOINTMENT_STATUS_TRANSITIONS 
} from '../../core/models/appointment.model';
import type { Pet } from '../../core/models/pet.model';
import type { Vet } from '../../core/models/vet.model';
import { useNotification } from '../../core/context/NotificationContext';
import PageHeader from '../../shared/components/PageHeader';
import ConfirmDialog from '../../shared/components/ConfirmDialog';
import AppointmentFormDialog from './AppointmentFormDialog';

const STATUS_KEYS = Object.keys(APPOINTMENT_STATUS_CONFIG) as AppointmentStatus[];

export default function AppointmentsPage() {
  const { success, error } = useNotification();
  const [all, setAll]           = useState<Appointment[]>([]);
  const [pets, setPets]         = useState<Pet[]>([]);
  const [vets, setVets]         = useState<Vet[]>([]);
  const [loading, setLoading]   = useState(false);
  const [formOpen, setFormOpen] = useState(false);
  const [editing, setEditing]   = useState<Appointment | undefined>();
  const [deleteTarget, setDeleteTarget] = useState<Appointment | undefined>();
  const [page, setPage]         = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [filterPet, setFilterPet]       = useState<number | ''>('');
  const [filterVet, setFilterVet]       = useState<number | ''>('');
  const [filterStatus, setFilterStatus] = useState<AppointmentStatus | ''>('');
  // Status menu
  const [menuAnchor, setMenuAnchor] = useState<null | HTMLElement>(null);
  const [menuTarget, setMenuTarget] = useState<Appointment | null>(null);

  const load = async () => {
    setLoading(true);
    try { setAll((await appointmentService.getAll()) ?? []); }
    catch { error('Error al cargar citas'); }
    finally { setLoading(false); }
  };

  useEffect(() => {
    load();
    petService.getAll().then(r => setPets(r ?? []));
    vetService.getAll().then(r => setVets(r ?? []));
  }, []);

  const filtered = useMemo(() => all.filter(a => {
    const matchPet    = !filterPet    || a.petId    === filterPet;
    const matchVet    = !filterVet    || a.vetId    === filterVet;
    const matchStatus = !filterStatus || a.status   === filterStatus;
    return matchPet && matchVet && matchStatus;
  }), [all, filterPet, filterVet, filterStatus]);

  const paged = filtered.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

  console.log({ all });

  const handleSave = async (data: AppointmentRequest) => {
    try {
      if (editing) { await appointmentService.update(editing.id, data); success('Cita actualizada'); }
      else         { await appointmentService.create(data);             success('Cita creada');     }
      setFormOpen(false); load();
    } catch { error('Error al guardar'); }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    try { await appointmentService.delete(deleteTarget.id); success('Cita eliminada'); load(); }
    catch { error('Error al eliminar'); }
    finally { setDeleteTarget(undefined); }
  };

  const handleChangeStatus = async (next: AppointmentStatus) => {
    if (!menuTarget) return;
    setMenuAnchor(null);
    try {
      await appointmentService.updateStatus(menuTarget.id, { status: next });
      success(`Cita marcada como: ${APPOINTMENT_STATUS_CONFIG[next].label}`);
      load();
    } catch { error('Error al cambiar el estado'); }
  };

  return (
    <Box>
      <PageHeader title="Citas" subtitle="Agenda de consultas veterinarias"
        showAddButton addButtonText="Nueva Cita"
        onAdd={() => { setEditing(undefined); setFormOpen(true); }} />

      {/* Filtros */}
      <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', mb: 2 }}>
        <TextField label="Mascota" select size="small" value={filterPet} sx={{ minWidth: 150 }}
          onChange={e => { setFilterPet(e.target.value === '' ? '' : Number(e.target.value)); setPage(0); }}>
          <MenuItem value="">Todas</MenuItem>
          {pets.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
        </TextField>
        <TextField label="Veterinario" select size="small" value={filterVet} sx={{ minWidth: 180 }}
          onChange={e => { setFilterVet(e.target.value === '' ? '' : Number(e.target.value)); setPage(0); }}>
          <MenuItem value="">Todos</MenuItem>
          {vets.map(v => <MenuItem key={v.id} value={v.id}>Dr. {v.firstName} {v.lastName}</MenuItem>)}
        </TextField>
        <TextField label="Estado" select size="small" value={filterStatus} sx={{ minWidth: 140 }}
          onChange={e => { setFilterStatus(e.target.value as AppointmentStatus | ''); setPage(0); }}>
          <MenuItem value="">Todos</MenuItem>
          {STATUS_KEYS.map(s => <MenuItem key={s} value={s}>{APPOINTMENT_STATUS_CONFIG[s].label}</MenuItem>)}
        </TextField>
      </Box>

      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', pt: 6 }}><CircularProgress /></Box>
      ) : (
        <Paper>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Fecha / Hora</TableCell>
                <TableCell>Mascota</TableCell>
                <TableCell>Veterinario</TableCell>
                <TableCell>Motivo</TableCell>
                <TableCell>Estado</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {paged.length === 0 ? (
                <TableRow><TableCell colSpan={6}>
                  <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
                    No hay citas registradas.
                  </Typography>
                </TableCell></TableRow>
              ) : paged.map(a => {
                const transitions = APPOINTMENT_STATUS_TRANSITIONS[a.status];
                // console.log(a);
                return (
                  <TableRow key={a.id} hover>
                    <TableCell>{new Date(a.scheduledAt).toLocaleString('es-CO')}</TableCell>
                    <TableCell>{`${a.petName  ?? '—'}`}</TableCell>
                    <TableCell>{a.vetName ? `Dr. ${a.vetName}` : '—'}</TableCell>
                    <TableCell>{a.reason}</TableCell>
                    <TableCell>
                      <Box component="span" sx={{
                        px: 1.5, py: 0.5, borderRadius: 2, fontSize: 12, fontWeight: 600,
                        backgroundColor: APPOINTMENT_STATUS_CONFIG[a.status].color + '22',
                        color: APPOINTMENT_STATUS_CONFIG[a.status].color,
                      }}>
                        {APPOINTMENT_STATUS_CONFIG[a.status].label}
                      </Box>
                    </TableCell>
                    <TableCell>
                      {transitions.length > 0 && (
                        <Tooltip title="Cambiar estado">
                          <IconButton color="secondary" onClick={e => { setMenuTarget(a); setMenuAnchor(e.currentTarget); }}>
                            <SwapHorizIcon />
                          </IconButton>
                        </Tooltip>
                      )}
                      <Tooltip title="Editar">
                        <span>
                          <IconButton color="primary"
                            disabled={a.status === 'Completed' || a.status === 'Cancelled'}
                            onClick={() => { setEditing(a); setFormOpen(true); }}>
                            <EditIcon />
                          </IconButton>
                        </span>
                      </Tooltip>
                      <Tooltip title="Eliminar">
                        <IconButton color="error" onClick={() => setDeleteTarget(a)}><DeleteIcon /></IconButton>
                      </Tooltip>
                    </TableCell>
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
          <TablePagination
            component="div" count={filtered.length} page={page} rowsPerPage={rowsPerPage}
            onPageChange={(_, p) => setPage(p)}
            onRowsPerPageChange={e => { setRowsPerPage(Number(e.target.value)); setPage(0); }}
            rowsPerPageOptions={[5, 10, 25]} labelRowsPerPage="Por página:" />
        </Paper>
      )}

      {/* Menú cambio de estado */}
      <Menu anchorEl={menuAnchor} open={!!menuAnchor} onClose={() => setMenuAnchor(null)}>
        {menuTarget && APPOINTMENT_STATUS_TRANSITIONS[menuTarget.status].map(next => (
          <MenuItem key={next} onClick={() => handleChangeStatus(next)}>
            {APPOINTMENT_STATUS_CONFIG[next].label}
          </MenuItem>
        ))}
      </Menu>

      <AppointmentFormDialog open={formOpen} appointment={editing} pets={pets} vets={vets}
        onClose={() => setFormOpen(false)} onSave={handleSave} />

      <ConfirmDialog
        open={!!deleteTarget}
        title="Eliminar cita"
        message={deleteTarget ? `¿Eliminar la cita del ${new Date(deleteTarget.scheduledAt).toLocaleDateString('es-CO')}?` : ''}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(undefined)} />
    </Box>
  );
}
