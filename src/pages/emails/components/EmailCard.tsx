import { useState } from 'react';
import {
  Avatar,
  Box,
  Chip,
  CircularProgress,
  IconButton,
  Menu,
  MenuItem,
  Paper,
  Stack,
  Typography,
} from '@mui/material';
import { DeleteOutline, MoreVert, StarOutline } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import type { Email } from '../../../api/types';
import { getEmailAvatarColor, getEmailAvatarLetter } from '../../../utils/emailAvatar';
import { getEmailStatus, statusStyles, type EmailStatus } from '../../../utils/emailStatus';

interface Props {
  email: Email;
  onDelete: (email: Email) => void;
  onConfirm: (email: Email) => void;
  onSetPrimary: (email: Email) => void;
  isDeleting: boolean;
  isConfirming: boolean;
  isSettingPrimary: boolean;
}

function statusLabelKey(status: EmailStatus): string {
  switch (status) {
    case 'confirmed':
      return 'emails.confirmed';
    case 'pending':
      return 'emails.pending';
    case 'unconfirmed':
      return 'emails.unconfirmed';
  }
}

export function EmailCard({
  email,
  onDelete,
  onConfirm,
  onSetPrimary,
  isDeleting,
  isConfirming,
  isSettingPrimary,
}: Props) {
  const { t } = useTranslation();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const menuOpen = Boolean(anchorEl);

  const status = getEmailStatus(email, isConfirming);
  const style = statusStyles[status];
  const avatarColors = getEmailAvatarColor(email.value);

  const handleCloseMenu = () => setAnchorEl(null);

  return (
    <Paper
      elevation={0}
      sx={{
        p: { xs: 1.5, sm: 2 },
        borderLeftWidth: email.isPrimary ? 4 : 0,
        borderLeftStyle: email.isPrimary ? 'solid' : undefined,
        borderLeftColor: email.isPrimary ? 'primary.main' : undefined,
        ...style.cardSx,
      }}
    >
      <Stack
        direction="row"
        alignItems={{ xs: 'flex-start', sm: 'center' }}
        justifyContent="space-between"
        spacing={1.5}
        sx={{ width: '100%' }}
      >
        <Stack
          direction="row"
          alignItems={{ xs: 'flex-start', sm: 'center' }}
          spacing={1.5}
          sx={{ flex: 1, minWidth: 0 }}
        >
          <Avatar
            sx={{
              width: 48,
              height: 48,
              flexShrink: 0,
              bgcolor: avatarColors.bg,
              color: avatarColors.text,
              fontSize: '1.25rem',
              fontWeight: 600,
            }}
          >
            {getEmailAvatarLetter(email.value)}
          </Avatar>
          <Box
            sx={{
              minWidth: 0,
              flex: 1,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'flex-start',
              gap: 0.5,
            }}
          >
            <Typography variant="body2" fontWeight={500} noWrap sx={{ minWidth: 0, width: '100%' }}>
              {email.value}
            </Typography>
            <Stack
              direction="row"
              alignItems="center"
              spacing={0.75}
              flexWrap="wrap"
              useFlexGap
            >
              <Chip
                size="small"
                label={t(statusLabelKey(status))}
                color={style.chipColor}
                variant={style.chipVariant}
              />
              {email.isPrimary && (
                <Chip
                  size="small"
                  label={t('emails.primary')}
                  color="primary"
                  variant="outlined"
                />
              )}
            </Stack>
          </Box>
        </Stack>

        <IconButton
          aria-label={t('emails.moreOptions')}
          onClick={(e) => setAnchorEl(e.currentTarget)}
          sx={{ flexShrink: 0, mt: -0.25 }}
        >
          <MoreVert sx={{ fontSize: 30 }} />
        </IconButton>
      </Stack>

      <Menu anchorEl={anchorEl} open={menuOpen} onClose={handleCloseMenu}>
        {!email.isConfirmed && (
          <MenuItem
            onClick={() => {
              handleCloseMenu();
              onConfirm(email);
            }}
            disabled={isConfirming}
          >
            {isConfirming ? (
              <CircularProgress size={18} sx={{ mr: 1 }} />
            ) : null}
            {t('emails.requestVerification')}
          </MenuItem>
        )}
        {email.isConfirmed && !email.isPrimary && (
          <MenuItem
            onClick={() => {
              handleCloseMenu();
              onSetPrimary(email);
            }}
            disabled={isSettingPrimary}
          >
            {isSettingPrimary ? (
              <CircularProgress size={18} sx={{ mr: 1 }} />
            ) : (
              <StarOutline sx={{ mr: 1, fontSize: 18 }} />
            )}
            {t('emails.makePrimary')}
          </MenuItem>
        )}
        {email.isPrimary && email.isConfirmed && (
          <MenuItem disabled>{t('emails.primaryAccountNotice')}</MenuItem>
        )}
        {!email.isPrimary && (
          <MenuItem
            onClick={() => {
              handleCloseMenu();
              onDelete(email);
            }}
            disabled={isDeleting}
            sx={{ color: 'error.main' }}
          >
            {isDeleting ? (
              <CircularProgress size={18} sx={{ mr: 1, color: 'error.main' }} />
            ) : (
              <DeleteOutline sx={{ mr: 1, fontSize: 18 }} />
            )}
            {t('common.delete')}
          </MenuItem>
        )}
      </Menu>
    </Paper>
  );
}
