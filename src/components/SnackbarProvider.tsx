/* eslint-disable react-refresh/only-export-components */
import {
  createContext,
  useCallback,
  useContext,
  useState,
  type ReactNode,
} from 'react';
import { Alert, Box, Snackbar, Typography, type AlertColor } from '@mui/material';
import type { ApiErrorSnackbarPayload } from '../api/apiError';

interface SnackbarContextValue {
  showSnackbar: (payload: ApiErrorSnackbarPayload, severity?: AlertColor) => void;
}

const SnackbarContext = createContext<SnackbarContextValue | null>(null);

interface SnackbarState {
  open: boolean;
  payload: ApiErrorSnackbarPayload;
  severity: AlertColor;
}

export function SnackbarProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<SnackbarState>({
    open: false,
    payload: '',
    severity: 'info',
  });

  const showSnackbar = useCallback(
    (payload: ApiErrorSnackbarPayload, severity: AlertColor = 'info') => {
      if (typeof payload === 'string') {
        const text =
          payload.trim().length > 0 ? payload.trim() : 'Something went wrong';
        setState({ open: true, payload: text, severity });
        return;
      }
      const body = payload.body?.trim() ?? '';
      if (!payload.title?.trim() && !body) {
        setState({ open: true, payload: 'Something went wrong', severity });
        return;
      }
      setState({ open: true, payload, severity });
    },
    []
  );

  const handleClose = () => {
    setState((prev) => ({ ...prev, open: false }));
  };

  return (
    <SnackbarContext.Provider value={{ showSnackbar }}>
      {children}
      <Snackbar
        open={state.open}
        autoHideDuration={
          typeof state.payload === 'object' &&
          state.payload.title &&
          state.payload.body
            ? 6000
            : 4000
        }
        onClose={handleClose}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <Alert onClose={handleClose} severity={state.severity} variant="filled">
          {typeof state.payload === 'string' ? (
            state.payload
          ) : (
            <Box sx={{ pr: 0.5 }}>
              {state.payload.title ? (
                <Typography variant="subtitle2" component="div" fontWeight={600}>
                  {state.payload.title}
                </Typography>
              ) : null}
              {state.payload.body ? (
                <Typography
                  variant="body2"
                  component="div"
                  sx={{
                    mt: state.payload.title ? 0.5 : 0,
                    opacity: state.payload.title ? 0.95 : 1,
                    whiteSpace: 'pre-line',
                  }}
                >
                  {state.payload.body}
                </Typography>
              ) : null}
            </Box>
          )}
        </Alert>
      </Snackbar>
    </SnackbarContext.Provider>
  );
}

export function useSnackbar(): SnackbarContextValue {
  const ctx = useContext(SnackbarContext);
  if (!ctx) throw new Error('useSnackbar must be used within SnackbarProvider');
  return ctx;
}
