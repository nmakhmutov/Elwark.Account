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
import { DeleteOutline, MoreVert } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import type { Connection, ExternalService } from '../../../api/types';
import { GoogleLogo } from '../../../components/GoogleLogo';
import { MicrosoftIcon } from '../../../components/MicrosoftIcon';
import { getStringAvatarColor, getStringAvatarLetter } from '../../../utils/emailAvatar';

/** Matches Identity social buttons: bordered tile + official logo. */
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

interface Props {
  connection: Connection;
  onDelete: (connection: Connection) => void;
  isDeleting: boolean;
}

function serviceLabelKey(type: ExternalService): string {
  switch (type) {
    case 'google':
      return 'connections.serviceGoogle';
    case 'microsoft':
      return 'connections.serviceMicrosoft';
    default:
      return 'connections.serviceUnknown';
  }
}

function ConnectionAvatar({ connection }: { connection: Connection }) {
  const { type, identity } = connection;

  if (type === 'google') {
    return (
      <Box sx={providerLogoSlotSx}>
        <GoogleLogo size={28} />
      </Box>
    );
  }

  if (type === 'microsoft') {
    return (
      <Box sx={providerLogoSlotSx}>
        <MicrosoftIcon sx={{ fontSize: 28 }} />
      </Box>
    );
  }

  const displayName = [connection.firstName, connection.lastName].filter(Boolean).join(' ').trim();
  const letterSource = displayName || identity;
  const colorKey = `${type}:${identity}`;
  const avatarColors = getStringAvatarColor(colorKey);

  return (
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
      {getStringAvatarLetter(letterSource)}
    </Avatar>
  );
}

export function ConnectionCard({ connection, onDelete, isDeleting }: Props) {
  const { t } = useTranslation();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const menuOpen = Boolean(anchorEl);

  const displayName = [connection.firstName, connection.lastName].filter(Boolean).join(' ').trim();
  const hasDisplayName = displayName.length > 0;
  const titleText = hasDisplayName ? displayName : t('connections.noDisplayName');

  const handleCloseMenu = () => setAnchorEl(null);

  return (
    <Paper
      elevation={0}
      sx={{
        p: { xs: 1.5, sm: 2 },
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
          <ConnectionAvatar connection={connection} />
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
            <Typography
              variant="body2"
              fontWeight={500}
              noWrap
              sx={{ minWidth: 0, width: '100%' }}
              color={hasDisplayName ? 'text.primary' : 'text.secondary'}
            >
              {titleText}
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
                label={t(serviceLabelKey(connection.type))}
                color="primary"
                variant="outlined"
              />
            </Stack>
          </Box>
        </Stack>

        <IconButton
          aria-label={t('connections.moreOptions')}
          onClick={(e) => setAnchorEl(e.currentTarget)}
          sx={{ flexShrink: 0, mt: -0.25 }}
        >
          <MoreVert sx={{ fontSize: 30 }} />
        </IconButton>
      </Stack>

      <Menu anchorEl={anchorEl} open={menuOpen} onClose={handleCloseMenu}>
        <MenuItem
          onClick={() => {
            handleCloseMenu();
            onDelete(connection);
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
      </Menu>
    </Paper>
  );
}
