import { Button, CircularProgress, type ButtonProps } from '@mui/material';

interface LoadingButtonProps extends ButtonProps {
  loading: boolean;
}

export function LoadingButton({ loading, disabled, children, ...props }: LoadingButtonProps) {
  return (
    <Button disabled={disabled || loading} {...props}>
      {loading ? <CircularProgress size={20} color="inherit" /> : children}
    </Button>
  );
}
