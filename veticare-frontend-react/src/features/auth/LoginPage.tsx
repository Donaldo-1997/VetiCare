import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import CircularProgress from '@mui/material/CircularProgress';
import PetsIcon from '@mui/icons-material/Pets';
import { authService } from '../../core/services/auth.service';
import { useAuth } from '../../core/context/AuthContext';
import { parseApiError } from '../../core/utils/error.utils';

export default function LoginPage() {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [email, setEmail]       = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading]   = useState(false);
  const [errorMsg, setErrorMsg] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErrorMsg('');
    setLoading(true);
    try {
      const { token } = await authService.login({ email, password });
      login(token);
      navigate('/dashboard', { replace: true });
    } catch (err) {
      setErrorMsg(parseApiError(err, 'Credenciales inválidas'));
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{
      minHeight: '100vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      bgcolor: '#1a237e',
    }}>
      <Card sx={{ width: '100%', maxWidth: 400, mx: 2 }}>
        <CardContent sx={{ p: 4 }}>
          {/* Logo */}
          <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', mb: 3 }}>
            <Box sx={{
              bgcolor: '#1a237e', borderRadius: '50%', p: 1.5, mb: 1,
              display: 'flex', alignItems: 'center', justifyContent: 'center',
            }}>
              <PetsIcon sx={{ color: '#fff', fontSize: 32 }} />
            </Box>
            <Typography variant="h5" sx={{ fontWeight: 700 }}>VetiCare</Typography>
            <Typography variant="body2" color="textSecondary">
              Sistema de gestión veterinaria
            </Typography>
          </Box>

          <form onSubmit={handleSubmit}>
            <TextField
              label="Correo electrónico"
              type="email"
              fullWidth
              required
              value={email}
              onChange={e => setEmail(e.target.value)}
              sx={{ mb: 2 }}
              autoComplete="email"
            />
            <TextField
              label="Contraseña"
              type="password"
              fullWidth
              required
              value={password}
              onChange={e => setPassword(e.target.value)}
              sx={{ mb: errorMsg ? 1 : 3 }}
              autoComplete="current-password"
            />

            {errorMsg && (
              <Typography color="error" variant="body2" sx={{ mb: 2 }}>
                {errorMsg}
              </Typography>
            )}

            <Button
              type="submit"
              variant="contained"
              fullWidth
              size="large"
              disabled={loading || !email || !password}
            >
              {loading ? <CircularProgress size={24} color="inherit" /> : 'Iniciar sesión'}
            </Button>
          </form>
        </CardContent>
      </Card>
    </Box>
  );
}
