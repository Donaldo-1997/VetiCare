import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import AddIcon from '@mui/icons-material/Add';

interface PageHeaderProps {
  title: string;
  subtitle?: string;
  showAddButton?: boolean;
  addButtonText?: string;
  onAdd?: () => void;
}

export default function PageHeader({
  title, subtitle, showAddButton, addButtonText = 'Nuevo', onAdd
}: PageHeaderProps) {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 3 }}>
      <Box>
        <Typography variant="h5" sx={{ fontWeight: 700 }}>{title}</Typography>
        {subtitle && (
          <Typography variant="body2" color="textSecondary" sx={{ mt: 0.5 }}>{subtitle}</Typography>
        )}
      </Box>
      {showAddButton && (
        <Button variant="contained" startIcon={<AddIcon />} onClick={onAdd}>
          {addButtonText}
        </Button>
      )}
    </Box>
  );
}
