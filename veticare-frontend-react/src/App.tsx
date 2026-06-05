import { useState } from 'react';
import { BrowserRouter, Routes, Route, NavLink, Navigate } from 'react-router-dom';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { es } from 'date-fns/locale';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import List from '@mui/material/List';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import PetsIcon from '@mui/icons-material/Pets';
import DashboardIcon from '@mui/icons-material/Dashboard';
import PeopleIcon from '@mui/icons-material/People';
import MedicalServicesIcon from '@mui/icons-material/MedicalServices';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import MedicationIcon from '@mui/icons-material/Medication';
import MenuIcon from '@mui/icons-material/Menu';

import { NotificationProvider } from './core/context/NotificationContext';
import Dashboard from './features/dashboard/Dashboard';
import OwnerList from './features/owners/OwnerList';
import PetList from './features/pets/PetList';
import PetDetail from './features/pets/PetDetail';
import VetList from './features/vets/VetList';
import AppointmentsPage from './features/appointments/AppointmentsPage';
import MedicineList from './features/medicines/MedicineList';

const DRAWER_WIDTH = 240;

const theme = createTheme({
  palette: {
    primary:   { main: '#1565C0' },
    secondary: { main: '#6a1b9a' },
  },
  shape: { borderRadius: 8 },
});

const navItems = [
  { path: '/dashboard',    label: 'Dashboard',    icon: <DashboardIcon /> },
  { path: '/owners',       label: 'Propietarios', icon: <PeopleIcon /> },
  { path: '/pets',         label: 'Mascotas',     icon: <PetsIcon /> },
  { path: '/vets',         label: 'Veterinarios', icon: <MedicalServicesIcon /> },
  { path: '/appointments', label: 'Citas',        icon: <CalendarMonthIcon /> },
  { path: '/medicines',    label: 'Medicamentos', icon: <MedicationIcon /> },
];

export default function App() {
  const [drawerOpen, setDrawerOpen] = useState(true);

  const drawer = (
    <Box sx={{ display: 'flex', flexDirection: 'column', height: '100%', bgcolor: '#1a237e' }}>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, p: 2.5 }}>
        <PetsIcon sx={{ color: '#fff', fontSize: 28 }} />
        <Typography variant="h6" sx={{ color: '#fff', fontWeight: 700 }}>VetiCare</Typography>
      </Box>
      <List sx={{ flex: 1, px: 1 }}>
        {navItems.map(item => (
          <ListItemButton
            key={item.path}
            component={NavLink}
            to={item.path}
            sx={{
              borderRadius: 2, mb: 0.5, color: 'rgba(255,255,255,0.7)',
              '&.active': { bgcolor: 'rgba(255,255,255,0.15)', color: '#fff' },
              '&:hover':  { bgcolor: 'rgba(255,255,255,0.1)',  color: '#fff' },
            }}
          >
            <ListItemIcon sx={{ color: 'inherit', minWidth: 36 }}>{item.icon}</ListItemIcon>
            <ListItemText primary={item.label} />
          </ListItemButton>
        ))}
      </List>
    </Box>
  );

  return (
    <ThemeProvider theme={theme}>
      <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={es}>
      <CssBaseline />
      <NotificationProvider>
        <BrowserRouter>
          <Box sx={{ display: 'flex' }}>
            <AppBar position="fixed" sx={{ zIndex: t => t.zIndex.drawer + 1 }}>
              <Toolbar>
                <IconButton color="inherit" edge="start" onClick={() => setDrawerOpen(o => !o)} sx={{ mr: 2 }}>
                  <MenuIcon />
                </IconButton>
                <Typography sx={{ fontWeight: 700 }} noWrap>
                  VetiCare
                </Typography>
              </Toolbar>
            </AppBar>

            <Drawer
              variant="persistent"
              open={drawerOpen}
              sx={{
                width: drawerOpen ? DRAWER_WIDTH : 0,
                flexShrink: 0,
                '& .MuiDrawer-paper': { width: DRAWER_WIDTH, boxSizing: 'border-box', border: 'none' },
              }}
            >
              <Toolbar />
              {drawer}
            </Drawer>

            <Box component="main" sx={{
              flexGrow: 1, p: 3, mt: 8,
              transition: 'margin 0.2s ease',
              minHeight: '100vh',
              bgcolor: '#f5f5f5',
            }}>
              <Routes>
                <Route path="/"             element={<Navigate to="/dashboard" replace />} />
                <Route path="/dashboard"    element={<Dashboard />} />
                <Route path="/owners"       element={<OwnerList />} />
                <Route path="/pets"         element={<PetList />} />
                <Route path="/pets/:id"     element={<PetDetail />} />
                <Route path="/vets"         element={<VetList />} />
                <Route path="/appointments" element={<AppointmentsPage />} />
                <Route path="/medicines"    element={<MedicineList />} />
                <Route path="*"             element={<Navigate to="/dashboard" replace />} />
              </Routes>
            </Box>
          </Box>
        </BrowserRouter>
      </NotificationProvider>
      </LocalizationProvider>
    </ThemeProvider>
  );
}
