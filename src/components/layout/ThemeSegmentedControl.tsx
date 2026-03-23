import { alpha, Box, ButtonBase, Tooltip, useTheme } from '@mui/material';
import LightModeIcon from '@mui/icons-material/LightMode';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import SettingsBrightnessIcon from '@mui/icons-material/SettingsBrightness';
import { useThemeMode } from '../../theme/ThemeContext';
import { useTranslation } from 'react-i18next';

type ThemeMode = 'system' | 'light' | 'dark';

const options: { value: ThemeMode; Icon: React.ElementType; labelKey: string }[] = [
  { value: 'system', Icon: SettingsBrightnessIcon, labelKey: 'theme.system' },
  { value: 'light', Icon: LightModeIcon, labelKey: 'theme.light' },
  { value: 'dark', Icon: DarkModeIcon, labelKey: 'theme.dark' },
];

/** Full-width strip for sidebar (edge-to-edge via parent `mx: -padding`). */
export function ThemeSegmentedControl() {
  const { mode, setMode } = useThemeMode();
  const { t } = useTranslation();
  const theme = useTheme();

  const trackBg =
    theme.palette.mode === 'light'
      ? theme.palette.grey[100]
      : alpha(theme.palette.common.white, 0.06);

  return (
    <Box
      sx={{
        display: 'flex',
        width: '100%',
        minWidth: 0,
        maxWidth: '100%',
        borderRadius: 0,
        py: 0.5,
        px: 0.75,
        gap: 0.25,
        bgcolor: trackBg,
        boxSizing: 'border-box',
        overflow: 'hidden',
      }}
    >
      {options.map(({ value, Icon, labelKey }) => {
        const selected = mode === value;
        return (
          <Tooltip key={value} title={t(labelKey)} placement="top">
            <ButtonBase
              aria-pressed={selected}
              onClick={() => setMode(value)}
              sx={{
                flex: 1,
                minWidth: 0,
                py: 0.75,
                borderRadius: 1.5,
                color: selected ? 'primary.main' : 'text.secondary',
                bgcolor: selected
                  ? theme.palette.mode === 'light'
                    ? theme.palette.common.white
                    : alpha(theme.palette.common.white, 0.1)
                  : 'transparent',
                boxShadow: selected
                  ? theme.palette.mode === 'light'
                    ? '0 1px 2px rgba(15, 23, 42, 0.08), 0 1px 1px rgba(15, 23, 42, 0.04)'
                    : '0 1px 3px rgba(0, 0, 0, 0.35)'
                  : 'none',
                transition: 'color 0.15s ease, background-color 0.15s ease, box-shadow 0.15s ease',
                '&:hover': {
                  color: selected ? 'primary.main' : 'text.primary',
                  bgcolor: selected
                    ? theme.palette.mode === 'light'
                      ? theme.palette.common.white
                      : alpha(theme.palette.common.white, 0.14)
                    : alpha(theme.palette.text.primary, 0.04),
                },
              }}
            >
              <Icon sx={{ fontSize: 18, opacity: selected ? 1 : 0.75 }} />
            </ButtonBase>
          </Tooltip>
        );
      })}
    </Box>
  );
}
