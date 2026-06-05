import { useEffect, useState, type JSX } from 'react';
import { useNavigate } from 'react-router-dom';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Table from '@mui/material/Table';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableBody from '@mui/material/TableBody';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import CircularProgress from '@mui/material/CircularProgress';
import Button from '@mui/material/Button';
import PeopleIcon from '@mui/icons-material/People';
import PetsIcon from '@mui/icons-material/Pets';
import MedicalServicesIcon from '@mui/icons-material/MedicalServices';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import { ownerService } from '../../core/services/owner.service';
import { petService } from '../../core/services/pet.service';
import { vetService } from '../../core/services/vet.service';
import { appointmentService } from '../../core/services/appointment.service';
import type { Appointment } from '../../core/models/appointment.model';
import { APPOINTMENT_STATUS_CONFIG } from '../../core/models/appointment.model';

interface StatCard { label: string; value: number; icon: JSX.Element; color: string; route: string; }

export default function Dashboard() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState<StatCard[]>([]);
  const [upcoming, setUpcoming] = useState<Appointment[]>([]);

  useEffect(() => {
    Promise.all([
      ownerService.getAll(),
      petService.getAll(),
      vetService.getAll(),
      appointmentService.getAll(),
    ]).then(([owners, pets, vets, appointments]) => {
      const scheduled = (appointments ?? []).filter(a => a.status === 'Scheduled');
      setStats([
        { label: 'Propietarios',    value: (owners ?? []).length,  icon: <PeopleIcon />,          color: '#1565C0', route: '/owners'       },
        { label: 'Mascotas',        value: (pets ?? []).length,    icon: <PetsIcon />,             color: '#2e7d32', route: '/pets'         },
        { label: 'Veterinarios',    value: (vets ?? []).length,    icon: <MedicalServicesIcon />,  color: '#6a1b9a', route: '/vets'         },
        { label: 'Citas pendientes',value: scheduled.length,       icon: <CalendarMonthIcon />,    color: '#e65100', route: '/appointments' },
      ]);
      setUpcoming(
        scheduled
          .sort((a, b) => new Date(a.scheduledAt).getTime() - new Date(b.scheduledAt).getTime())
          .slice(0, 10)
      );
    }).finally(() => setLoading(false));
  }, []);

  if (loading) return (
    <Box sx={{ display: 'flex', justifyContent: 'center', pt: 8 }}><CircularProgress /></Box>
  );

  return (
    <Box>
      <Typography variant="h5" sx={{ fontWeight: 700 }}>Dashboard</Typography>
      <Typography variant="body2" color="textSecondary" sx={{ mb: 3 }}>Resumen general de VetiCare</Typography>

      {/* Stat cards */}
      <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: 2, mb: 3 }}>
        {stats.map(card => (
          <Card key={card.label} sx={{ cursor: 'pointer', '&:hover': { boxShadow: 4 } }}
            onClick={() => navigate(card.route)}>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                <Box sx={{
                  width: 52, height: 52, borderRadius: 2, display: 'flex',
                  alignItems: 'center', justifyContent: 'center',
                  backgroundColor: card.color + '22', color: card.color, flexShrink: 0,
                }}>
                  {card.icon}
                </Box>
                <Box>
                  <Typography variant="h4" sx={{ fontWeight: 700, lineHeight: 1 }}>{card.value}</Typography>
                  <Typography variant="caption" color="textSecondary">{card.label}</Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        ))}
      </Box>

      {/* Próximas citas */}
      <Card>
        <CardContent>
          <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <CalendarMonthIcon color="primary" />
              <Typography variant="h6">Próximas citas agendadas</Typography>
            </Box>
            <Button size="small" onClick={() => navigate('/appointments')}>Ver todas</Button>
          </Box>

          {upcoming.length === 0 ? (
            <Typography color="textSecondary" sx={{ textAlign: 'center', py: 3 }}>
              No hay citas agendadas próximamente.
            </Typography>
          ) : (
            <TableContainer>
            <Table size="small" sx={{ minWidth: 600 }}>
              <TableHead>
                <TableRow>
                  <TableCell>Fecha</TableCell>
                  <TableCell>Mascota</TableCell>
                  <TableCell>Veterinario</TableCell>
                  <TableCell>Motivo</TableCell>
                  <TableCell>Estado</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {upcoming.map(a => (
                  <TableRow key={a.id}>
                    <TableCell>{new Date(a.scheduledAt).toLocaleString('es-CO')}</TableCell>
                    <TableCell>{`${a.petName ?? '—'}`}</TableCell>
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
                  </TableRow>
                ))}
              </TableBody>
            </Table>
            </TableContainer>
          )}
        </CardContent>
      </Card>
    </Box>
  );
}
