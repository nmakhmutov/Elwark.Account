import { useState } from 'react';
import {
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
import { AppleLogo } from '../../../components/AppleLogo';
import { GoogleLogo } from '../../../components/GoogleLogo';
import { MicrosoftIcon } from '../../../components/MicrosoftIcon';
import { ProtonLogo } from '../../../components/ProtonLogo';
import { YahooLogo } from '../../../components/YahooLogo';
import { getStringAvatarColor, getStringAvatarLetter } from '../../../utils/emailAvatar';
import { getEmailDomain, getEmailProviderKind } from '../../../utils/emailProviderDomain';
import { getEmailStatus, statusStyles, type EmailStatus } from '../../../utils/emailStatus';

/** Same bordered tile as ConnectionCard / Identity social buttons. */
const providerLogoSlotSx = {
  width: 48,
  height: 48,
  flexShrink: 0,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  borderRadius: 2,
  border: '1px solid',
  borderColor: 'divider',
  bgcolor: 'background.paper',
} as const;

function EmailDomainAvatar({ email }: { email: string }) {
  const kind = getEmailProviderKind(email);
  const domain = getEmailDomain(email);

  switch (kind) {
    case 'google':
      return (
        <Box sx={providerLogoSlotSx}>
          <GoogleLogo size={28} />
        </Box>
      );
    case 'microsoft':
      return (
        <Box sx={providerLogoSlotSx}>
          <MicrosoftIcon size={28} />
        </Box>
      );
    case 'yahoo':
      return (
        <Box sx={providerLogoSlotSx}>
          <YahooLogo size={28} />
        </Box>
      );
    case 'apple':
      return (
        <Box sx={providerLogoSlotSx}>
          <AppleLogo size={28} />
        </Box>
      );
    case 'proton':
      return (
        <Box sx={providerLogoSlotSx}>
          <ProtonLogo size={28} />
        </Box>
      );
    case 'other': {
      const key = domain || email;
      const avatarColors = getStringAvatarColor(key);
      return (
        <Box
          sx={{
            ...providerLogoSlotSx,
            bgcolor: avatarColors.bg,
          }}
        >
          <Typography
            component="span"
            sx={{
              fontSize: '1.25rem',
              fontWeight: 600,
              lineHeight: 1,
              color: avatarColors.text,
            }}
          >
            {getStringAvatarLetter(key)}
          </Typography>
        </Box>
      );
    }
  }
}

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

  const handleCloseMenu = () => setAnchorEl(null);

  return (
    <Paper
      elevation={0}
      sx={{
        p: { xs: 1.5, sm: 2 },
        ...(email.isPrimary
          ? {
              borderLeftWidth: 4,
              borderLeftStyle: 'solid',
              borderLeftColor: 'primary.main',
            }
          : {}),
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
          <EmailDomainAvatar email={email.value} />
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
