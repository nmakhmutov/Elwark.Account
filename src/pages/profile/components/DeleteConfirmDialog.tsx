import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { LoadingButton } from '../../../components/LoadingButton';

interface Props {
  open: boolean;
  title: string;
  message: string;
  confirmText: string;
  onConfirm: () => void;
  onCancel: () => void;
  isConfirmLoading?: boolean;
}

export function DeleteConfirmDialog({
  open,
  title,
  message,
  confirmText,
  onConfirm,
  onCancel,
  isConfirmLoading = false,
}: Props) {
  const { t } = useTranslation();

  return (
    <Dialog open={open} onClose={onCancel}>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <DialogContentText>{message}</DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onCancel} disabled={isConfirmLoading}>
          {t('common.no')}
        </Button>
        <LoadingButton loading={isConfirmLoading} onClick={onConfirm} color="error">
          {confirmText}
        </LoadingButton>
      </DialogActions>
    </Dialog>
  );
}
