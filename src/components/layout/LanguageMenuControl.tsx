import { useState, type MouseEvent } from 'react';
import {
  Box,
  FormControl,
  IconButton,
  Menu,
  MenuItem,
  Select,
  Tooltip,
  type SelectChangeEvent,
} from '@mui/material';
import type { SxProps, Theme } from '@mui/material/styles';
import { KeyboardArrowDown, Translate as TranslateIcon } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';

const LANGUAGES: Record<string, string> = { en: 'English', ru: 'Русский' };

type Props = {
  /** Called after choosing a language (e.g. close mobile drawer). */
  onLanguageSelected?: () => void;
  /** MUI IconButton size (`variant="icon"` only). */
  size?: 'small' | 'medium' | 'large';
  sx?: SxProps<Theme>;
  /** Full-width borderless Select + icon (sidebar); default is compact icon + Menu. */
  variant?: 'icon' | 'row';
};

/** Language picker: icon + Menu, or borderless Select row with translate icon. */
export function LanguageMenuControl({
  onLanguageSelected,
  size = 'small',
  sx,
  variant = 'icon',
}: Props) {
  const [anchor, setAnchor] = useState<HTMLElement | null>(null);
  const { i18n, t } = useTranslation();

  const currentLabel = LANGUAGES[i18n.language] ?? i18n.language;

  const handleOpen = (e: MouseEvent<HTMLElement>) => {
    setAnchor(e.currentTarget);
  };

  const handleClose = () => {
    setAnchor(null);
  };

  const handleSelect = (code: string) => {
    i18n.changeLanguage(code);
    handleClose();
    onLanguageSelected?.();
  };

  const menuOpen = Boolean(anchor);
  const chevronSize = size === 'small' ? 18 : 20;

  if (variant === 'row') {
    const resolved = i18n.resolvedLanguage ?? i18n.language;
    const base = resolved.split('-')[0];
    const value = base in LANGUAGES ? base : 'en';

    const handleChange = (e: SelectChangeEvent<string>) => {
      i18n.changeLanguage(e.target.value);
      onLanguageSelected?.();
    };

    return (
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, width: '100%', minWidth: 0 }}>
        <TranslateIcon sx={{ fontSize: 20, color: 'text.secondary', flexShrink: 0 }} aria-hidden />
        <FormControl fullWidth size="small" variant="standard" sx={{ m: 0, minWidth: 0 }}>
          <Select
            variant="standard"
            disableUnderline
            value={value}
            onChange={handleChange}
            inputProps={{ 'aria-label': t('common.language') }}
            sx={{
              '& .MuiSelect-select': {
                display: 'flex',
                alignItems: 'center',
                py: 0.5,
                pl: 0,
                pr: 2.5,
              },
            }}
          >
            {Object.entries(LANGUAGES).map(([code, name]) => (
              <MenuItem key={code} value={code}>
                {name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>
    );
  }

  return (
    <>
      <Tooltip title={currentLabel}>
        <IconButton
          onClick={handleOpen}
          size={size}
          color="inherit"
          aria-label={currentLabel}
          aria-haspopup="true"
          aria-expanded={menuOpen ? 'true' : undefined}
          sx={[
            { flexShrink: 0, borderRadius: 1.5, px: 0.75 },
            ...(sx === undefined ? [] : Array.isArray(sx) ? sx : [sx]),
          ]}
        >
          <Box
            component="span"
            sx={{
              display: 'inline-flex',
              alignItems: 'center',
              gap: 0.125,
            }}
          >
            <TranslateIcon fontSize={size === 'small' ? 'small' : 'medium'} />
            <KeyboardArrowDown
              aria-hidden
              sx={{
                fontSize: chevronSize,
                opacity: 0.8,
                transition: 'transform 0.2s ease',
                transform: menuOpen ? 'rotate(180deg)' : 'none',
              }}
            />
          </Box>
        </IconButton>
      </Tooltip>
      <Menu
        anchorEl={anchor}
        open={menuOpen}
        onClose={handleClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
        slotProps={{ paper: { sx: { minWidth: 140 } } }}
      >
        {Object.entries(LANGUAGES).map(([code, name]) => (
          <MenuItem key={code} selected={i18n.language === code} onClick={() => handleSelect(code)}>
            {name}
          </MenuItem>
        ))}
      </Menu>
    </>
  );
}
