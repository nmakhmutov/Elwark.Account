import { useState } from 'react';
import { Box, Paper, Stack, Typography } from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import { apiErrorSnackbarPayload } from '../../api/apiError';
import { useAccount, useDeleteConnection } from '../../api/hooks/useAccount';
import { LoadingScreen } from '../../components/LoadingScreen';
import { useSnackbar } from '../../components/SnackbarProvider';
import { ConnectionCard } from './components/ConnectionCard';
import { DeleteConfirmDialog } from '../profile/components/DeleteConfirmDialog';
import type { Connection } from '../../api/types';

export function ConnectionsPage() {
  const { t } = useTranslation();
  const { data: account, isLoading } = useAccount();
  const { showSnackbar } = useSnackbar();
  const deleteConnection = useDeleteConnection();

  const [deleteConnTarget, setDeleteConnTarget] = useState<Connection | null>(null);
  const [deletingConn, setDeletingConn] = useState<string | null>(null);

  if (isLoading || !account) {
    return <LoadingScreen />;
  }

  const addConnectionHref = (() => {
    const authority = import.meta.env.VITE_OIDC_AUTHORITY;
    const returnUrl = encodeURIComponent(window.location.href);
    return `${authority}/append?returnUrl=${returnUrl}`;
  })();

  const confirmDeleteConnection = () => {
    if (!deleteConnTarget) return;
    const service = deleteConnTarget.type;
    const connKey = `${service}:${deleteConnTarget.identity}`;
    setDeletingConn(connKey);
    deleteConnection.mutate(
      { service, identity: deleteConnTarget.identity },
      {
        onSuccess: () => {
          setDeletingConn(null);
          setDeleteConnTarget(null);
        },
        onError: (err) => {
          showSnackbar(apiErrorSnackbarPayload(err), 'error');
          setDeletingConn(null);
          setDeleteConnTarget(null);
        },
      }
    );
  };

  const hasConnections = account.connections.length > 0;
  const deleteTargetKey =
    deleteConnTarget !== null
      ? `${deleteConnTarget.type}:${deleteConnTarget.identity}`
      : '';

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
      <Box>
        <Typography variant="h6" fontWeight={500} sx={{ fontSize: { xs: 16, md: 18 } }}>
          {t('connections.title')}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {t('connections.subtitle')}
        </Typography>
      </Box>

      {hasConnections ? (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>
          {account.connections.map((conn) => {
            const connKey = `${conn.type}:${conn.identity}`;
            return (
              <ConnectionCard
                key={connKey}
                connection={conn}
                onDelete={(c) => setDeleteConnTarget(c)}
                isDeleting={deletingConn === connKey}
              />
            );
          })}
        </Box>
      ) : (
        <Paper elevation={0} sx={{ p: 4 }}>
          <Stack alignItems="center" spacing={2}>
            <Box
              component="img"
              src="/images/empty_social_connections.svg"
              alt={t('connections.empty')}
              sx={{ width: 200 }}
            />
            <Typography align="center" color="text.secondary">
              {t('connections.empty')}
            </Typography>
          </Stack>
        </Paper>
      )}

      <Paper
        component="a"
        href={addConnectionHref}
        variant="outlined"
        sx={{
          p: 1.5,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          gap: 1,
          cursor: 'pointer',
          borderStyle: 'dashed',
          borderColor: 'primary.main',
          color: 'primary.main',
          textDecoration: 'none',
          '&:hover': {
            borderColor: 'primary.main',
            color: 'primary.main',
            bgcolor: 'action.hover',
          },
        }}
      >
        <AddIcon sx={{ fontSize: 18 }} />
        <Typography variant="body2">
          {hasConnections ? t('connections.addAnother') : t('connections.addConnection')}
        </Typography>
      </Paper>

      <DeleteConfirmDialog
        open={Boolean(deleteConnTarget)}
        title={t('connections.deleteTitle', { service: deleteConnTarget?.type ?? '' })}
        message={t('connections.deleteMessage', {
          service: deleteConnTarget?.type ?? '',
          identity: deleteConnTarget?.identity ?? '',
        })}
        confirmText={t('common.delete')}
        onConfirm={confirmDeleteConnection}
        onCancel={() => setDeleteConnTarget(null)}
        isConfirmLoading={deleteConnTarget !== null && deletingConn === deleteTargetKey}
      />
    </Box>
  );
}
