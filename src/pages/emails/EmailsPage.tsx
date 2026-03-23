import { useState } from 'react';
import { Box, Button, Typography } from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import { useAccount, useDeleteEmail, useRequestEmailVerification, useSetPrimaryEmail } from '../../api/hooks/useAccount';
import { LoadingScreen } from '../../components/LoadingScreen';
import { useSnackbar } from '../../components/SnackbarProvider';
import { EmailCard } from './components/EmailCard';
import { EmailAddDialog } from '../profile/components/EmailAddDialog';
import { EmailConfirmDialog } from '../profile/components/EmailConfirmDialog';
import { DeleteConfirmDialog } from '../profile/components/DeleteConfirmDialog';
import { apiErrorSnackbarPayload } from '../../api/apiError';
import type { Email } from '../../api/types';

export function EmailsPage() {
  const { t } = useTranslation();
  const { data: account, isLoading } = useAccount();
  const { showSnackbar } = useSnackbar();

  const deleteEmail = useDeleteEmail();
  const requestVerification = useRequestEmailVerification();
  const setPrimaryEmail = useSetPrimaryEmail();

  const [addEmailOpen, setAddEmailOpen] = useState(false);
  const [confirmDialogOpen, setConfirmDialogOpen] = useState(false);
  const [confirmToken, setConfirmToken] = useState('');
  const [deleteEmailTarget, setDeleteEmailTarget] = useState<Email | null>(null);
  const [confirmingEmail, setConfirmingEmail] = useState<string | null>(null);
  const [settingPrimaryEmail, setSettingPrimaryEmail] = useState<string | null>(null);
  const [deletingEmail, setDeletingEmail] = useState<string | null>(null);

  if (isLoading || !account) {
    return <LoadingScreen />;
  }

  const handleConfirmEmail = (email: Email) => {
    setConfirmingEmail(email.value);
    requestVerification.mutate(
      { email: email.value },
      {
        onSuccess: (data) => {
          setConfirmToken(data.token);
          setConfirmDialogOpen(true);
          setConfirmingEmail(null);
        },
        onError: () => {
          setConfirmingEmail(null);
        },
      }
    );
  };

  const handleSetPrimary = (email: Email) => {
    setSettingPrimaryEmail(email.value);
    setPrimaryEmail.mutate(
      { email: email.value },
      {
        onSuccess: () => setSettingPrimaryEmail(null),
        onError: (err) => {
          showSnackbar(apiErrorSnackbarPayload(err), 'error');
          setSettingPrimaryEmail(null);
        },
      }
    );
  };

  const confirmDeleteEmail = () => {
    if (!deleteEmailTarget) return;
    setDeletingEmail(deleteEmailTarget.value);
    deleteEmail.mutate(deleteEmailTarget.value, {
      onSuccess: () => {
        setDeletingEmail(null);
        setDeleteEmailTarget(null);
      },
      onError: (err) => {
        showSnackbar(apiErrorSnackbarPayload(err), 'error');
        setDeletingEmail(null);
        setDeleteEmailTarget(null);
      },
    });
  };

  const hasEmails = account.emails.length > 0;
  const addDisabled = account.emails.some((e) => !e.isConfirmed);

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
      <Box>
        <Typography variant="h6" fontWeight={500} sx={{ fontSize: { xs: 16, md: 18 } }}>
          {t('emails.title')}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {t('emails.subtitle')}
        </Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>
        {account.emails.map((email) => (
          <EmailCard
            key={email.value}
            email={email}
            onDelete={(e) => setDeleteEmailTarget(e)}
            onConfirm={handleConfirmEmail}
            onSetPrimary={handleSetPrimary}
            isDeleting={deletingEmail === email.value}
            isConfirming={confirmingEmail === email.value}
            isSettingPrimary={settingPrimaryEmail === email.value}
          />
        ))}
      </Box>

      <Button
        variant="outlined"
        fullWidth
        disabled={addDisabled}
        onClick={() => setAddEmailOpen(true)}
        startIcon={<AddIcon sx={{ fontSize: 18 }} />}
        sx={{
          py: 1.5,
          borderStyle: 'dashed',
          borderColor: addDisabled ? 'action.disabled' : 'primary.main',
          color: addDisabled ? 'action.disabled' : 'primary.main',
          bgcolor: addDisabled ? 'action.hover' : 'transparent',
          '&:hover': addDisabled
            ? undefined
            : {
                borderColor: 'primary.main',
                color: 'primary.main',
                bgcolor: 'action.hover',
              },
        }}
      >
        {hasEmails ? t('emails.addAnother') : t('emails.addEmail')}
      </Button>

      <EmailAddDialog open={addEmailOpen} onClose={() => setAddEmailOpen(false)} />
      <EmailConfirmDialog
        open={confirmDialogOpen}
        token={confirmToken}
        onClose={() => setConfirmDialogOpen(false)}
      />
      <DeleteConfirmDialog
        open={Boolean(deleteEmailTarget)}
        title={t('emails.deleteTitle')}
        message={t('emails.deleteMessage', { email: deleteEmailTarget?.value ?? '' })}
        confirmText={t('common.delete')}
        onConfirm={confirmDeleteEmail}
        onCancel={() => setDeleteEmailTarget(null)}
        isConfirmLoading={
          deleteEmailTarget !== null && deletingEmail === deleteEmailTarget.value
        }
      />
    </Box>
  );
}
