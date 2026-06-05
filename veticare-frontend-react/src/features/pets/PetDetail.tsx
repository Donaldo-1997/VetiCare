import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import CircularProgress from '@mui/material/CircularProgress';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Table from '@mui/material/Table';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { petService } from '../../core/services/pet.service';
import { medicalRecordService } from '../../core/services/medical-record.service';
import { appointmentService } from '../../core/services/appointment.service';
import type { Pet } from '../../core/models/pet.model';
import type { MedicalRecord, MedicalRecordRequest } from '../../core/models/medical-record.model';
import type { Appointment } from '../../core/models/appointment.model';
import { APPOINTMENT_STATUS_CONFIG } from '../../core/models/appointment.model';
import { useNotification } from '../../core/context/NotificationContext';
import ConfirmDialog from '../../shared/components/ConfirmDialog';
import MedicalRecordFormDialog from '../medical-records/MedicalRecordFormDialog';
import { parseApiError } from '../../core/utils/error.utils';

export default function PetDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { success, error } = useNotification();
  const [pet, setPet]               = useState<Pet | null>(null);
  const [records, setRecords]       = useState<MedicalRecord[]>([]);
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [loading, setLoading]       = useState(true);
  const [tab, setTab]               = useState(0);
  const [recordFormOpen, setRecordFormOpen] = useState(false);
  const [editingRecord, setEditingRecord]   = useState<MedicalRecord | undefined>();
  const [deleteTarget, setDeleteTarget]     = useState<MedicalRecord | undefined>();

  const petId = Number(id);

  const loadRecords = () => medicalRecordService.getByPet(petId).then(r => setRecords(r ?? []));
  const loadAppointments = () => appointmentService.getByPet(petId).then(r => setAppointments(r ?? []));

  useEffect(() => {
    setLoading(true);
    petService.getById(petId).then(p => {
      setPet(p);
      setLoading(false);
      loadRecords();
      loadAppointments();
    }).catch(() => setLoading(false));
  }, [petId]);

  const handleSaveRecord = async (data: MedicalRecordRequest) => {
    try {
      if (editingRecord) { await medicalRecordService.update(editingRecord.id, data); success('Registro actualizado'); }
      else               { await medicalRecordService.create(data);                   success('Registro creado');     }
      setRecordFormOpen(false); loadRecords();
    } catch (err) { error(parseApiError(err, 'Error al guardar el registro médico')); }
  };

  const handleDeleteRecord = async () => {
    if (!deleteTarget) return;
    try { await medicalRecordService.delete(deleteTarget.id); success('Registro eliminado'); loadRecords(); }
    catch (err) { error(parseApiError(err, 'Error al eliminar el registro médico')); }
    finally { setDeleteTarget(undefined); }
  };

  if (loading) return <Box sx={{ display: 'flex', justifyContent: 'center', pt: 8 }}><CircularProgress /></Box>;
  if (!pet) return <Typography>Mascota no encontrada.</Typography>;

  const eligibleAppointments = appointments.filter(a => a.status === 'Completed' || a.status === 'InProgress');

  return (
    <Box>
      {/* Header */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 3 }}>
        <Tooltip title="Volver">
          <IconButton onClick={() => navigate('/pets')}><ArrowBackIcon /></IconButton>
        </Tooltip>
        <Box>
          <Typography variant="h5" sx={{ fontWeight: 700 }}>{pet.name}</Typography>
          <Typography variant="body2" color="textSecondary">
            {`${pet.breedName ?? 'Raza desconocida'}`} · {pet.gender === 'Male' ? 'Macho' : 'Hembra'} · {pet.weight} kg
          </Typography>
        </Box>
      </Box>

      <Tabs value={tab} onChange={(_, v) => setTab(v)} sx={{ mb: 2 }}>
        <Tab label="Información" />
        <Tab label={`Historial médico (${records.length})`} />
        <Tab label={`Citas (${appointments.length})`} />
      </Tabs>

      {/* Tab 0: Info */}
      {tab === 0 && (
        <Card>
          <CardContent>
            <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: 2 }}>
              {[
                ['Propietario', pet.ownerName ? `${pet.ownerName}` : '—'],
                ['Fecha de nacimiento', new Date(pet.birthDate).toLocaleDateString('es-CO')],
                ['Peso', `${pet.weight} kg`],
                ['Raza', pet.breedName ?? '—'],
                ['Género', pet.gender === 'Male' ? 'Macho' : 'Hembra'],
                ['Registrado', new Date(pet.createdAt).toLocaleDateString('es-CO')],
              ].map(([label, value]) => (
                <Box key={`${label}`}>
                  <Typography variant="caption" color="textSecondary" sx={{ textTransform: 'uppercase', letterSpacing: 0.5 }}>
                    {`${label}`}
                  </Typography>
                  <Typography sx={{ fontWeight: 500 }}>{`${value}`}</Typography>
                </Box>
              ))}
            </Box>
          </CardContent>
        </Card>
      )}

      {/* Tab 1: Historial médico */}
      {tab === 1 && (
        <Box>
          <Box sx={{ mb: 2 }}>
            <Button variant="contained" startIcon={<AddIcon />}
              onClick={() => { setEditingRecord(undefined); setRecordFormOpen(true); }}>
              Nuevo Registro
            </Button>
          </Box>
          {records.length === 0 ? (
            <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
              No hay registros médicos para esta mascota.
            </Typography>
          ) : records.map(r => (
            <Card key={r.id} sx={{ mb: 2 }}>
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                  <Box>
                    <Typography sx={{ fontWeight: 700 }}>{new Date(r.createdAt).toLocaleDateString('es-CO')}</Typography>
                    <Typography variant="caption" color="textSecondary">Cita #{r.appointmentId}</Typography>
                  </Box>
                  <Box>
                    <Tooltip title="Editar">
                      <IconButton color="primary" onClick={() => { setEditingRecord(r); setRecordFormOpen(true); }}>
                        <EditIcon />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Eliminar">
                      <IconButton color="error" onClick={() => setDeleteTarget(r)}><DeleteIcon /></IconButton>
                    </Tooltip>
                  </Box>
                </Box>
                <Typography sx={{ mt: 1 }}><strong>Diagnóstico:</strong> {r.diagnosis}</Typography>
                <Typography><strong>Tratamiento:</strong> {r.treatment}</Typography>
                {r.notes && <Typography><strong>Notas:</strong> {r.notes}</Typography>}

                {r.prescriptions && r.prescriptions.length > 0 && (
                  <>
                    <Divider sx={{ my: 1.5 }} />
                    <Typography variant="caption" color="textSecondary">PRESCRIPCIONES</Typography>
                    <Table size="small" sx={{ mt: 1 }}>
                      <TableHead>
                        <TableRow>
                          <TableCell>Medicamento</TableCell>
                          <TableCell>Dosis</TableCell>
                          <TableCell>Cantidad</TableCell>
                          <TableCell>Instrucciones</TableCell>
                        </TableRow>
                      </TableHead>
                      <TableBody>
                        {r.prescriptions.map(p => (
                          <TableRow key={p.id}>
                            <TableCell>{p.medicine?.name ?? '—'}</TableCell>
                            <TableCell>{p.dosage}</TableCell>
                            <TableCell>{p.quantity}</TableCell>
                            <TableCell>{p.instructions}</TableCell>
                          </TableRow>
                        ))}
                      </TableBody>
                    </Table>
                  </>
                )}
              </CardContent>
            </Card>
          ))}
        </Box>
      )}

      {/* Tab 2: Citas */}
      {tab === 2 && (
        <Card>
          <CardContent>
            {appointments.length === 0 ? (
              <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
                No hay citas para esta mascota.
              </Typography>
            ) : (
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Fecha</TableCell>
                    <TableCell>Motivo</TableCell>
                    <TableCell>Estado</TableCell>
                    <TableCell>Veterinario</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {appointments.map(a => (
                    <TableRow key={a.id}>
                      <TableCell>{new Date(a.scheduledAt).toLocaleString('es-CO')}</TableCell>
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
                      <TableCell>{a.vetName ? `Dr. ${a.vetName}` : '—'}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      )}

      <MedicalRecordFormDialog
        open={recordFormOpen} record={editingRecord} petId={petId}
        appointments={eligibleAppointments}
        onClose={() => setRecordFormOpen(false)} onSave={handleSaveRecord} />

      <ConfirmDialog
        open={!!deleteTarget}
        title="Eliminar registro"
        message="¿Eliminar este registro médico?"
        onConfirm={handleDeleteRecord}
        onCancel={() => setDeleteTarget(undefined)} />
    </Box>
  );
}
