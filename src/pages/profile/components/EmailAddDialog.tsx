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
import { useAddEmail } from '../../../api/hooks/useAccount';

interface Props {
  open: boolean;
  onClose: () => void;
}

export function EmailAddDialog({ open, onClose }: Props) {
  const { t } = useTranslation();
  const [email, setEmail] = useState('');
  const [error, setError] = useState<string | null>(null);
  const addEmail = useAddEmail();

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!email.trim()) {
      setError(t('emails.validation.required'));
      return;
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setError(t('emails.validation.invalidEmail'));
      return;
    }

    addEmail.mutate(
      { email },
      {
        onSuccess: () => {
          setEmail('');
          onClose();
        },
        onError: (err) => {
          setError(formatApiError(err));
        },
      }
    );
  };

  const handleClose = () => {
    setEmail('');
    setError(null);
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogContent sx={{ p: { xs: 2, sm: 4 } }}>
        <Typography variant="h6" align="center" sx={{ mb: { xs: 2, sm: 3 } }}>
          {t('emails.addNew')}
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label={t('emails.email')}
            value={email}
            onChange={(e) => setEmail(e.target.value)}
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
              loading={addEmail.isPending}
              type="submit"
              variant="contained"
              color="primary"
            >
              {t('common.add')}
            </LoadingButton>
            <Button onClick={handleClose}>{t('common.cancel')}</Button>
          </Stack>
        </form>
      </DialogContent>
    </Dialog>
  );
}
