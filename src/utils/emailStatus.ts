import type { SxProps, Theme } from '@mui/material/styles';
import type { Email } from '../api/types';

export type EmailStatus = 'confirmed' | 'pending' | 'unconfirmed';

export const normalizeEmailStatus = (apiStatus: string): EmailStatus => {
  const lower = apiStatus.toLowerCase();
  if (lower === 'confirmed') return 'confirmed';
  if (lower === 'pending') return 'pending';
  return 'unconfirmed';
};

/** Maps API booleans + client “verification request in flight” to canonical status for UI. */
export const getEmailStatus = (
  email: Email,
  isRequestingVerification: boolean
): EmailStatus => {
  if (email.isConfirmed) return 'confirmed';
  if (isRequestingVerification) return 'pending';
  return 'unconfirmed';
};

/** Visual styles for status chips and card chrome — keep palette-driven; no stray hex in components. */
export const statusStyles: Record<
  EmailStatus,
  {
    chipColor: 'success' | 'warning' | 'default';
    chipVariant: 'filled' | 'outlined';
    cardSx: SxProps<Theme>;
  }
> = {
  confirmed: {
    chipColor: 'success',
    chipVariant: 'filled',
    cardSx: {},
  },
  pending: {
    chipColor: 'warning',
    chipVariant: 'filled',
    cardSx: {},
  },
  unconfirmed: {
    chipColor: 'warning',
    chipVariant: 'outlined',
    cardSx: { opacity: 0.65 },
  },
};
