import { createContext, useContext, useState, useCallback } from 'react';
import type { ReactNode } from 'react';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

interface NotificationContextType {
  success: (msg: string) => void;
  error:   (msg: string) => void;
}

const NotificationContext = createContext<NotificationContextType>({
  success: () => {},
  error:   () => {},
});

export function useNotification() {
  return useContext(NotificationContext);
}

export function NotificationProvider({ children }: { children: ReactNode }) {
  const [open, setOpen]       = useState(false);
  const [message, setMessage] = useState('');
  const [severity, setSeverity] = useState<'success' | 'error'>('success');

  const success = useCallback((msg: string) => {
    setMessage(msg); setSeverity('success'); setOpen(true);
  }, []);

  const error = useCallback((msg: string) => {
    setMessage(msg); setSeverity('error'); setOpen(true);
  }, []);

  return (
    <NotificationContext.Provider value={{ success, error }}>
      {children}
      <Snackbar
        open={open}
        autoHideDuration={3500}
        onClose={() => setOpen(false)}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert onClose={() => setOpen(false)} severity={severity} variant="filled">
          {message}
        </Alert>
      </Snackbar>
    </NotificationContext.Provider>
  );
}
