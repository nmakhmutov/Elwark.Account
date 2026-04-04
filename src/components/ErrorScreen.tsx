import { Box, Button, Paper, Stack, Typography } from '@mui/material';

interface ErrorScreenProps {
  title?: string;
  message: string;
  details?: string | null;
  actionLabel?: string;
  onAction?: () => void;
}

export function ErrorScreen({
  title = 'Something went wrong',
  message,
  details,
  actionLabel = 'Try again',
  onAction,
}: ErrorScreenProps) {
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
        <Typography variant="body2" color="text.secondary">
          {message}
        </Typography>
        {details ? (
          <Box
            component="pre"
            sx={{
              p: 1.5,
              mt: 2,
              mb: 0,
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
        {onAction ? (
          <Stack direction={{ xs: 'column', sm: 'row' }} spacing={1.5} sx={{ mt: 2 }}>
            <Button variant="contained" onClick={onAction}>
              {actionLabel}
            </Button>
          </Stack>
        ) : null}
      </Paper>
    </Box>
  );
}
