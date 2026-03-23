import { isRouteErrorResponse, useNavigate, useRouteError } from 'react-router-dom';
import { Box, Button, Paper, Stack, Typography } from '@mui/material';

/**
 * Shown when a route throws (render, loader, action). Wired via `errorElement` on the router.
 */
export function RouteErrorFallback() {
  const error = useRouteError();
  const navigate = useNavigate();

  let title = 'Something went wrong';
  let message = 'An unexpected error occurred. You can try reloading the page or going back home.';
  let details = '';

  if (isRouteErrorResponse(error)) {
    title = `${error.status} ${error.statusText || 'Error'}`.trim();
    const data = error.data;
    message =
      typeof data === 'string'
        ? data
        : data != null && typeof data === 'object' && 'message' in data
          ? String((data as { message: unknown }).message)
          : error.statusText || message;
  } else if (error instanceof Error) {
    message = error.message;
    details = import.meta.env.DEV ? (error.stack ?? '') : '';
  } else {
    message = String(error);
  }

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        p: 2,
        bgcolor: 'background.default',
      }}
    >
      <Paper
        elevation={0}
        sx={{
          p: 3,
          maxWidth: 560,
          width: '100%',
          border: 1,
          borderColor: 'divider',
          borderRadius: 2,
        }}
      >
        <Typography variant="h6" component="h1" gutterBottom>
          {title}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
          {message}
        </Typography>
        {import.meta.env.DEV && details ? (
          <Box
            component="pre"
            sx={{
              p: 1.5,
              mb: 2,
              borderRadius: 1,
              bgcolor: 'action.hover',
              overflow: 'auto',
              fontSize: '0.75rem',
              fontFamily: 'ui-monospace, monospace',
              whiteSpace: 'pre-wrap',
              wordBreak: 'break-word',
            }}
          >
            {details}
          </Box>
        ) : null}
        <Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.5}>
          <Button variant="contained" onClick={() => window.location.reload()}>
            Reload page
          </Button>
          <Button variant="outlined" onClick={() => navigate('/', { replace: true })}>
            Go to home
          </Button>
        </Stack>
      </Paper>
    </Box>
  );
}
