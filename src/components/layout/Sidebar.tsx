import { useState, type MouseEvent } from 'react';
import {
  Avatar,
  Box,
  ButtonBase,
  Divider,
  ListItemButton,
  Popover,
  Typography,
} from '@mui/material';
import {
  Person as PersonIcon,
  Email as EmailIcon,
  Link as LinkIcon,
  Logout as LogoutIcon,
  KeyboardArrowDown,
} from '@mui/icons-material';
import { NavLink, useLocation } from 'react-router-dom';
import { useAuth } from 'react-oidc-context';
import { useTranslation } from 'react-i18next';
import { useAccount } from '../../api/hooks/useAccount';
import { ThemeSegmentedControl } from './ThemeSegmentedControl';
import { LanguageMenuControl } from './LanguageMenuControl';
import { AppBrandMark } from './AppBrandMark';

/** Locale-aware date in the user's profile timezone (IANA id). */
function formatAccountCreatedDisplay(iso: string, timezone: string, locale: string): string | null {
  const date = new Date(iso);
  if (Number.isNaN(date.getTime())) return null;
  try {
    return new Intl.DateTimeFormat(locale, {
      dateStyle: 'long',
      timeZone: timezone,
    }).format(date);
  } catch {
    return null;
  }
}

interface Props {
  onClose?: () => void;
}

/** Fallback avatar tint (visible on light grey sidebar cards). */
const AVATAR_FALLBACK_BG = '#7c3aed';

/** Full-width row hover (edge-to-edge inside popover). */
const accountMenuRowSx = {
  borderRadius: 0,
  py: 1.25,
  px: 2,
  minHeight: 44,
  width: '100%',
  mx: 0,
  alignItems: 'center',
  gap: 0,
  '&:hover': {
    bgcolor: 'action.hover',
  },
} as const;

export function Sidebar({ onClose }: Props) {
  const auth = useAuth();
  const { t, i18n } = useTranslation();
  const location = useLocation();
  const { data: account } = useAccount();
  const [userMenuAnchor, setUserMenuAnchor] = useState<null | HTMLElement>(null);
  const userMenuOpen = Boolean(userMenuAnchor);

  const navItems = [
    { path: '/', label: t('nav.profile'), icon: <PersonIcon sx={{ fontSize: 22 }} /> },
    { path: '/emails', label: t('nav.emails'), icon: <EmailIcon sx={{ fontSize: 22 }} /> },
    { path: '/connections', label: t('nav.connections'), icon: <LinkIcon sx={{ fontSize: 22 }} /> },
  ];

  const displayName =
    account?.fullName ??
    auth.user?.profile.given_name ??
    auth.user?.profile.preferred_username ??
    '';

  const primaryEmail = account?.emails.find((e) => e.isPrimary)?.value
    ?? auth.user?.profile.email
    ?? '';

  const initials = displayName ? displayName[0].toUpperCase() : '?';

  const memberSinceLine =
    account &&
    (() => {
      const date = formatAccountCreatedDisplay(
        account.createdAt,
        account.timezone,
        i18n.language
      );
      if (!date) return null;
      return t('profile.memberSince', { date });
    })();

  const openUserMenu = (e: MouseEvent<HTMLElement>) => {
    setUserMenuAnchor(e.currentTarget);
  };

  const closeUserMenu = () => {
    setUserMenuAnchor(null);
  };

  const handleSignOut = () => {
    closeUserMenu();
    onClose?.();
    auth.signoutRedirect();
  };

  return (
    <Box
      sx={{
        width: '100%',
        maxWidth: '100%',
        minWidth: 0,
        display: 'flex',
        flexDirection: 'column',
        height: '100%',
        p: 1.25,
        boxSizing: 'border-box',
        overflow: 'hidden',
      }}
    >
      <ButtonBase
        onClick={openUserMenu}
        aria-haspopup="dialog"
        aria-expanded={userMenuOpen ? 'true' : undefined}
        aria-label={displayName || primaryEmail || t('nav.account')}
        disableRipple
        sx={{
          alignSelf: 'stretch',
          display: 'flex',
          alignItems: 'center',
          gap: 1,
          mt: -1.25,
          mx: -1.25,
          mb: 0,
          pt: 2,
          pb: 0.75,
          px: 1.25,
          borderRadius: 0,
          textAlign: 'left',
          color: 'inherit',
          boxSizing: 'border-box',
          '&:hover': {
            bgcolor: 'action.hover',
          },
          ...(userMenuOpen
            ? {
                bgcolor: 'action.selected',
              }
            : {}),
        }}
      >
        <Avatar
          src={account?.picture}
          slotProps={{ img: { alt: '' } }}
          sx={{
            width: 40,
            height: 40,
            fontSize: '1rem',
            fontWeight: 500,
            color: '#ffffff',
            bgcolor: account?.picture ? 'transparent' : AVATAR_FALLBACK_BG,
            flexShrink: 0,
          }}
        >
          {initials}
        </Avatar>
        <Box sx={{ minWidth: 0, flex: 1, overflow: 'hidden' }}>
          <Typography
            variant="body2"
            fontWeight={600}
            noWrap
            sx={{ color: 'text.primary', lineHeight: 1.3 }}
          >
            {displayName || '—'}
          </Typography>
          <Typography
            variant="caption"
            noWrap
            display="block"
            sx={{ color: 'text.secondary', mt: 0.25 }}
          >
            {primaryEmail || '—'}
          </Typography>
        </Box>
        <KeyboardArrowDown
          aria-hidden
          sx={{
            flexShrink: 0,
            fontSize: 22,
            color: 'text.secondary',
            opacity: 0.85,
            transition: 'transform 0.2s ease',
            transform: userMenuOpen ? 'rotate(180deg)' : 'none',
          }}
        />
      </ButtonBase>

      <Popover
        open={userMenuOpen}
        anchorEl={userMenuAnchor}
        onClose={closeUserMenu}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'left' }}
        transformOrigin={{ vertical: 'top', horizontal: 'left' }}
        slotProps={{
          paper: {
            elevation: 8,
            sx: {
              width: 300,
              maxWidth: 'min(300px, calc(100vw - 24px))',
              p: 0,
              overflow: 'hidden',
              borderRadius: 2,
              border: 1,
              borderColor: 'divider',
              mt: 0.5,
              boxSizing: 'border-box',
            },
          },
        }}
      >
        <Box sx={{ py: 1.5, px: 2, borderBottom: 1, borderColor: 'divider' }}>
          <AppBrandMark logoHeight={24} titleVariant="subtitle1" />
        </Box>

        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            gap: 1.5,
            py: 2,
            px: 2,
            borderBottom: 1,
            borderColor: 'divider',
          }}
        >
          <Avatar
            src={account?.picture}
            slotProps={{ img: { alt: '' } }}
            sx={{
              width: 48,
              height: 48,
              fontSize: '1.125rem',
              fontWeight: 600,
              color: '#ffffff',
              bgcolor: account?.picture ? 'transparent' : AVATAR_FALLBACK_BG,
              flexShrink: 0,
            }}
          >
            {initials}
          </Avatar>
          <Box sx={{ minWidth: 0, flex: 1 }}>
            <Typography variant="subtitle2" fontWeight={600} noWrap>
              {displayName || '—'}
            </Typography>
            <Typography variant="caption" color="text.secondary" display="block" noWrap sx={{ mt: 0.25 }}>
              {primaryEmail || '—'}
            </Typography>
            {memberSinceLine ? (
              <Typography variant="caption" color="text.secondary" display="block" sx={{ mt: 0.75 }}>
                {memberSinceLine}
              </Typography>
            ) : null}
          </Box>
        </Box>

        <Divider sx={{ my: 0 }} />

        <ListItemButton onClick={handleSignOut} sx={{ ...accountMenuRowSx, color: 'error.main' }}>
          <LogoutIcon sx={{ fontSize: 20, mr: 1.5, flexShrink: 0 }} />
          <Typography variant="body2" fontWeight={500}>
            {t('common.signOut')}
          </Typography>
        </ListItemButton>
      </Popover>

      <Divider sx={{ mx: -1.25, my: 0, mb: 1.5 }} />

      {/* Navigation */}
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.375 }}>
        {navItems.map(({ path, label, icon }) => {
          const active = location.pathname === path;
          return (
            <ListItemButton
              key={path}
              component={NavLink}
              to={path}
              onClick={onClose}
              selected={active}
              sx={{
                borderRadius: 1.5,
                gap: 1.25,
                py: 0.75,
                px: 1.25,
                minHeight: 40,
                '&.Mui-selected': {
                  bgcolor: 'action.selected',
                  color: 'primary.main',
                },
                '&.Mui-selected:hover': {
                  bgcolor: 'action.selected',
                },
              }}
            >
              <Box
                sx={{
                  color: active ? 'primary.main' : 'text.secondary',
                  display: 'flex',
                }}
              >
                {icon}
              </Box>
              <Typography variant="body2" fontWeight={active ? 500 : 400} sx={{ fontSize: '0.9375rem' }}>
                {label}
              </Typography>
            </ListItemButton>
          );
        })}
      </Box>

      {/* Spacer pushes bottom section down */}
      <Box sx={{ flex: 1 }} />

      {/* Bottom: language + theme */}
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.75 }}>
        <Box sx={{ mb: 1.25, alignSelf: 'stretch' }}>
          <LanguageMenuControl variant="row" onLanguageSelected={onClose} />
        </Box>
        <Box
          sx={{
            mx: -1.25,
            mb: -1.25,
            alignSelf: 'stretch',
            minWidth: 0,
            overflow: 'hidden',
          }}
        >
          <ThemeSegmentedControl />
        </Box>
      </Box>
    </Box>
  );
}
