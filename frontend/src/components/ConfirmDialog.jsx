import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  IconButton,
} from '@mui/material';

import CloseIcon from '@mui/icons-material/Close';
import WarningAmberIcon from '@mui/icons-material/WarningAmber';

export default function ConfirmDialog({
  open,
  title = 'Confirmar ação',
  message,
  confirmText = 'Confirmar',
  cancelText = 'Cancelar',
  onConfirm,
  onClose,
  loading = false,
}) {
  return (
    <Dialog
      open={open}
      onClose={onClose}
      fullWidth
      maxWidth="xs"
    >
      <DialogTitle
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          gap: 1,
        }}
      >
        <span
          style={{
            display: 'flex',
            alignItems: 'center',
            gap: 8,
          }}
        >
          <WarningAmberIcon color="error" />
          {title}
        </span>

        <IconButton
          onClick={onClose}
          size="small"
          aria-label="Fechar"
        >
          <CloseIcon />
        </IconButton>
      </DialogTitle>

      <DialogContent dividers>
        <DialogContentText>
          {message}
        </DialogContentText>
      </DialogContent>

      <DialogActions>
        <Button
          onClick={onClose}
          color="inherit"
          disabled={loading}
        >
          {cancelText}
        </Button>

        <Button
          onClick={onConfirm}
          color="error"
          variant="contained"
          disabled={loading}
        >
          {loading ? 'Processando...' : confirmText}
        </Button>
      </DialogActions>
    </Dialog>
  );
}