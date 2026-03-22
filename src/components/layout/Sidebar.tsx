import { useState, type MouseEvent } from 'react';
import {
  alpha,
  Avatar,
  Box,
  Divider,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Menu,
  MenuItem,
  Typography,
  useTheme,
} from '@mui/material';
import {
  Person as PersonIcon,
  Email as EmailIcon,
  Link as LinkIcon,
  Logout as LogoutIcon,
  Translate as TranslateIcon,
  KeyboardArrowDown,
} from '@mui/icons-material';
import { NavLink, useLocation } from 'react-router-dom';
import { useAuth } from 'react-oidc-context';
import { useTranslation } from 'react-i18next';
import { useAccount } from '../../api/hooks/useAccount';
import { ThemeSegmentedControl } from './ThemeSegmentedControl';

const LANGUAGES: Record<string, string> = { en: 'English', ru: 'Русский' };

interface Props {
  onClose?: () => void;
}

/** Fallback avatar tint (visible on light grey sidebar cards). */
const AVATAR_FALLBACK_BG = '#7c3aed';

export function Sidebar({ onClose }: Props) {
  const theme = useTheme();
  const auth = useAuth();
  const { t, i18n } = useTranslation();
  const location = useLocation();
  const { data: account } = useAccount();

  const [langAnchor, setLangAnchor] = useState<HTMLElement | null>(null);

  const navItems = [
    { path: '/', label: t('nav.profile'), icon: <PersonIcon sx={{ fontSize: 20 }} /> },
    { path: '/emails', label: t('nav.emails'), icon: <EmailIcon sx={{ fontSize: 20 }} /> },
    { path: '/connections', label: t('nav.connections'), icon: <LinkIcon sx={{ fontSize: 20 }} /> },
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

  const handleLangOpen = (e: MouseEvent<HTMLElement>) => {
    setLangAnchor(e.currentTarget);
  };

  const handleLangClose = () => {
    setLangAnchor(null);
  };

  const handleLangSelect = (lang: string) => {
    i18n.changeLanguage(lang);
    handleLangClose();
  };

  return (
    <Box
      sx={{
        width: 240,
        display: 'flex',
        flexDirection: 'column',
        height: '100%',
        p: 2,
        overflowX: 'hidden',
      }}
    >
      {/* User profile block — distinct surface in light mode (same issue as theme control: white-on-white) */}
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          gap: 1.5,
          p: 1.5,
          borderRadius: 2,
          border: '1px solid',
          borderColor: 'divider',
          bgcolor:
            theme.palette.mode === 'light'
              ? theme.palette.grey[100]
              : alpha(theme.palette.common.white, 0.06),
        }}
      >
        <Avatar
          src={account?.picture}
          imgProps={{ alt: '' }}
          sx={{
            width: 40,
            height: 40,
            fontSize: '1rem',
            fontWeight: 600,
            color: '#ffffff',
            bgcolor: account?.picture ? 'transparent' : AVATAR_FALLBACK_BG,
          }}
        >
          {initials}
        </Avatar>
        <Box sx={{ minWidth: 0, overflow: 'hidden' }}>
          <Typography
            variant="body2"
            fontWeight={600}
            noWrap
            sx={{ color: 'text.primary', lineHeight: 1.3 }}
          >
            {displayName}
          </Typography>
          <Typography
            variant="caption"
            noWrap
            display="block"
            sx={{ color: 'text.secondary', mt: 0.25 }}
          >
            {primaryEmail}
          </Typography>
        </Box>
      </Box>

      {/* Theme switcher — right below user block */}
      <Box sx={{ mt: 1, mb: 2 }}>
        <ThemeSegmentedControl />
      </Box>

      {/* Navigation — compact, no flex:1 */}
      <Typography
        variant="caption"
        color="text.disabled"
        sx={{
          px: 1.5,
          pb: 0.5,
          textTransform: 'uppercase',
          letterSpacing: '0.05em',
          fontWeight: 500,
        }}
      >
        {t('nav.account')}
      </Typography>

      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
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
                borderRadius: 2,
                gap: 1.5,
                py: 0.75,
                px: 1.5,
                minHeight: 0,
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
              <Typography variant="body2" fontWeight={active ? 500 : 400}>
                {label}
              </Typography>
            </ListItemButton>
          );
        })}
      </Box>

      {/* Spacer pushes bottom section down */}
      <Box sx={{ flex: 1 }} />

      {/* Bottom section */}
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
        {/* Language dropdown */}
        <ListItemButton
          onClick={handleLangOpen}
          sx={{
            borderRadius: 2,
            py: 0.75,
            px: 1.5,
            minHeight: 0,
          }}
        >
          <ListItemIcon sx={{ minWidth: 0, mr: 1.5, color: 'text.secondary' }}>
            <TranslateIcon sx={{ fontSize: 20 }} />
          </ListItemIcon>
          <ListItemText
            primary={LANGUAGES[i18n.language] ?? i18n.language}
            primaryTypographyProps={{ variant: 'body2' }}
          />
          <KeyboardArrowDown sx={{ fontSize: 18, color: 'text.secondary' }} />
        </ListItemButton>
        <Menu
          anchorEl={langAnchor}
          open={Boolean(langAnchor)}
          onClose={handleLangClose}
          anchorOrigin={{ vertical: 'top', horizontal: 'left' }}
          transformOrigin={{ vertical: 'bottom', horizontal: 'left' }}
        >
          {Object.entries(LANGUAGES).map(([code, name]) => (
            <MenuItem
              key={code}
              selected={i18n.language === code}
              onClick={() => handleLangSelect(code)}
            >
              {name}
            </MenuItem>
          ))}
        </Menu>

        <Divider sx={{ my: 0.5 }} />

        {/* Sign out */}
        <ListItemButton
          onClick={() => auth.signoutRedirect()}
          sx={{
            borderRadius: 2,
            py: 0.75,
            px: 1.5,
            minHeight: 0,
            color: 'error.main',
            '&:hover': { bgcolor: 'error.main', color: 'error.contrastText' },
          }}
        >
          <ListItemIcon sx={{ minWidth: 0, mr: 1.5, color: 'inherit' }}>
            <LogoutIcon sx={{ fontSize: 18 }} />
          </ListItemIcon>
          <ListItemText
            primary={t('common.signOut')}
            primaryTypographyProps={{ variant: 'body2' }}
          />
        </ListItemButton>
      </Box>
    </Box>
  );
}
