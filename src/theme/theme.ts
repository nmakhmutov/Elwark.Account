import { alpha, createTheme } from '@mui/material/styles';

/**
 * Visual language from Elwark.Identity `Identity.Api` (Tailwind zinc palette):
 * zinc-900 CTAs, zinc-200 borders, zinc-500 secondary text, white bg, rounded-lg inputs.
 */
const fontFamily = [
  'ui-sans-serif',
  'system-ui',
  '-apple-system',
  'BlinkMacSystemFont',
  '"Segoe UI"',
  'Roboto',
  '"Helvetica Neue"',
  'Arial',
  '"Noto Sans"',
  'sans-serif',
].join(',');

/** Tailwind zinc scale (aligned with Identity.Api pages). */
const zinc = {
  50: '#fafafa',
  100: '#f4f4f5',
  200: '#e4e4e7',
  300: '#d4d4d8',
  400: '#a1a1aa',
  500: '#71717a',
  600: '#52525b',
  700: '#3f3f46',
  800: '#27272a',
  900: '#18181b',
  950: '#09090b',
} as const;

export function createAppTheme(mode: 'light' | 'dark') {
  const isLight = mode === 'light';

  return createTheme({
    palette: {
      mode,
      ...(isLight
        ? {
            primary: {
              main: zinc[900],
              light: zinc[50],
              dark: zinc[700],
              contrastText: '#ffffff',
            },
            secondary: {
              main: zinc[600],
              light: zinc[200],
              dark: zinc[800],
            },
            success: {
              main: '#16a34a',
              light: '#f0fdf4',
              dark: '#15803d',
              contrastText: '#ffffff',
            },
            info: {
              main: zinc[600],
              light: zinc[100],
              dark: zinc[800],
              contrastText: '#ffffff',
            },
            error: {
              main: '#ef4444',
              light: '#fef2f2',
              dark: '#b91c1c',
              contrastText: '#ffffff',
            },
            warning: {
              main: '#f59e0b',
              light: '#fffbeb',
              dark: '#b45309',
              contrastText: '#1c1917',
            },
            grey: {
              50: zinc[50],
              100: zinc[100],
              200: zinc[200],
              300: zinc[300],
              400: zinc[400],
              500: zinc[500],
              600: zinc[600],
              700: zinc[700],
              800: zinc[800],
              900: zinc[900],
              A100: zinc[100],
              A200: zinc[200],
              A400: zinc[400],
              A700: zinc[700],
            },
            text: {
              primary: zinc[900],
              secondary: zinc[500],
              disabled: zinc[400],
            },
            action: {
              active: zinc[900],
              hover: alpha(zinc[900], 0.04),
              selected: alpha(zinc[900], 0.08),
              disabledBackground: alpha(zinc[900], 0.12),
            },
            divider: zinc[200],
            background: {
              default: '#ffffff',
              paper: '#ffffff',
            },
          }
        : {
            primary: {
              main: zinc[50],
              light: zinc[100],
              dark: zinc[300],
              contrastText: zinc[900],
            },
            secondary: {
              main: zinc[400],
              light: zinc[600],
              dark: zinc[300],
            },
            success: {
              main: '#22c55e',
              light: alpha('#22c55e', 0.16),
              dark: '#16a34a',
              contrastText: '#052e16',
            },
            info: {
              main: zinc[400],
              light: alpha(zinc[400], 0.16),
              dark: zinc[500],
              contrastText: zinc[950],
            },
            error: {
              main: '#f87171',
              light: alpha('#f87171', 0.16),
              dark: '#ef4444',
              contrastText: zinc[950],
            },
            warning: {
              main: '#fbbf24',
              light: alpha('#fbbf24', 0.16),
              dark: '#f59e0b',
              contrastText: zinc[950],
            },
            grey: {
              50: zinc[950],
              100: zinc[900],
              200: zinc[800],
              300: zinc[700],
              400: zinc[600],
              500: zinc[500],
              600: zinc[400],
              700: zinc[300],
              800: zinc[200],
              900: zinc[100],
              A100: zinc[800],
              A200: zinc[700],
              A400: zinc[500],
              A700: zinc[300],
            },
            text: {
              primary: zinc[50],
              secondary: zinc[400],
              disabled: zinc[500],
            },
            action: {
              active: zinc[50],
              hover: alpha('#ffffff', 0.05),
              selected: alpha('#ffffff', 0.08),
              disabledBackground: alpha('#ffffff', 0.12),
            },
            divider: zinc[800],
            background: {
              default: zinc[950],
              paper: zinc[900],
            },
          }),
    },
    shape: { borderRadius: 8 },
    typography: {
      fontFamily,
      h1: {
        fontWeight: 600,
        fontSize: '1.5rem',
        lineHeight: '2rem',
        letterSpacing: '-0.025em',
        color: isLight ? zinc[900] : zinc[50],
      },
      h2: {
        fontWeight: 600,
        fontSize: '1.375rem',
        lineHeight: '1.75rem',
        letterSpacing: '-0.02em',
      },
      h3: {
        fontWeight: 600,
        fontSize: '1.25rem',
        lineHeight: '1.5rem',
      },
      h4: {
        fontWeight: 600,
        fontSize: '1.125rem',
        lineHeight: '1.5rem',
      },
      h5: {
        fontWeight: 600,
        fontSize: '1rem',
        lineHeight: '1.5rem',
      },
      h6: {
        fontWeight: 600,
        fontSize: '0.9375rem',
        lineHeight: '1.4rem',
      },
      button: {
        textTransform: 'none',
        fontWeight: 500,
        fontSize: '0.875rem',
      },
      body1: {
        fontSize: '0.875rem',
        lineHeight: '1.25rem',
        fontWeight: 400,
      },
      body2: {
        fontSize: '0.75rem',
        lineHeight: '1rem',
        fontWeight: 400,
      },
      subtitle1: {
        fontSize: '0.875rem',
        fontWeight: 400,
      },
      subtitle2: {
        fontSize: '0.875rem',
        fontWeight: 400,
      },
    },
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            WebkitFontSmoothing: 'antialiased',
            MozOsxFontSmoothing: 'grayscale',
          },
          root: {
            '& .MuiPaper-elevation8, & .MuiPaper-elevation9, & .MuiPopover-root .MuiPaper-root': {
              boxShadow:
                '0 10px 15px -3px rgba(0, 0, 0, 0.08), 0 4px 6px -4px rgba(0, 0, 0, 0.06)',
            },
          },
        },
      },
      MuiButton: {
        defaultProps: { disableElevation: true },
        styleOverrides: {
          root: {
            borderRadius: 8,
            minHeight: 44,
          },
          sizeSmall: {
            minHeight: 36,
            fontSize: '0.8125rem',
          },
        },
      },
      MuiCard: {
        defaultProps: { elevation: 0 },
        styleOverrides: {
          root: {
            borderRadius: 8,
            border: 'none',
            boxShadow: 'none',
          },
        },
      },
      MuiCardContent: {
        styleOverrides: {
          root: ({ theme }) => ({
            padding: theme.spacing(2),
            '&:last-child': {
              paddingBottom: theme.spacing(2),
            },
          }),
        },
      },
      MuiPaper: {
        defaultProps: { elevation: 0 },
        styleOverrides: {
          root: ({ theme }) => ({
            border: `1px solid ${theme.palette.divider}`,
            backgroundImage: 'none',
          }),
        },
      },
      MuiOutlinedInput: {
        styleOverrides: {
          root: ({ theme }) => ({
            borderRadius: 8,
            '&:hover .MuiOutlinedInput-notchedOutline': {
              borderColor: theme.palette.divider,
            },
            '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
              borderWidth: 2,
            },
          }),
        },
      },
      MuiTextField: {
        defaultProps: { variant: 'outlined', size: 'small' },
      },
      MuiSelect: {
        defaultProps: { size: 'small' },
      },
    },
  });
}
