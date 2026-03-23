import { useState, type FormEvent } from 'react';
import {
  Dialog,
  DialogContent,
  TextField,
  Stack,
  Typography,
  Button,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { LoadingButton } from '../../../components/LoadingButton';
import { formatApiError } from '../../../api/apiError';
import { useConfirmEmail } from '../../../api/hooks/useAccount';

interface Props {
  open: boolean;
  token: string;
  onClose: () => void;
}

export function EmailConfirmDialog({ open, token, onClose }: Props) {
  const { t } = useTranslation();
  const [code, setCode] = useState('');
  const [error, setError] = useState<string | null>(null);
  const confirmEmail = useConfirmEmail();

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!code.trim()) {
      setError(t('emails.codeRequired'));
      return;
    }

    confirmEmail.mutate(
      { token, code },
      {
        onSuccess: () => {
          setCode('');
          onClose();
        },
        onError: (err) => {
          setError(formatApiError(err));
        },
      }
    );
  };

  const handleClose = () => {
    setCode('');
    setError(null);
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogContent sx={{ p: { xs: 2, sm: 4 } }}>
        <Typography variant="h6" align="center" sx={{ mb: { xs: 2, sm: 3 } }}>
          {t('emails.confirmEmail')}
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label={t('emails.code')}
            value={code}
            onChange={(e) => setCode(e.target.value)}
            error={Boolean(error)}
            helperText={error}
          />
          <Stack
            direction="row"
            justifyContent="center"
            alignItems="center"
            spacing={2}
            sx={{ mt: { xs: 2, sm: 3 } }}
          >
            <LoadingButton
              loading={confirmEmail.isPending}
              type="submit"
              variant="contained"
              color="primary"
            >
              {t('common.confirm')}
            </LoadingButton>
            <Button onClick={handleClose}>{t('common.cancel')}</Button>
          </Stack>
        </form>
      </DialogContent>
    </Dialog>
  );
}
