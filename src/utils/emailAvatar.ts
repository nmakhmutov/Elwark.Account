interface AvatarColor {
  bg: string;
  text: string;
}

const COLORS: AvatarColor[] = [
  { bg: '#ede9fe', text: '#3730a3' },
  { bg: '#fef3c7', text: '#d97706' },
  { bg: '#e0e7ff', text: '#4338ca' },
  { bg: '#dcfce7', text: '#15803d' },
  { bg: '#fee2e2', text: '#dc2626' },
  { bg: '#e0f2fe', text: '#0369a1' },
  { bg: '#fce7f3', text: '#be185d' },
  { bg: '#f0fdf4', text: '#166534' },
];

export const getEmailAvatarColor = (email: string): AvatarColor => {
  const domain = email.split('@')[1] ?? email;
  const index = domain.split('').reduce((acc, c) => acc + c.charCodeAt(0), 0);
  return COLORS[index % COLORS.length];
};

export const getEmailAvatarLetter = (email: string): string => {
  const domain = email.split('@')[1] ?? email;
  return domain[0]?.toUpperCase() ?? '?';
};

/** Deterministic pastel avatar for any string key (e.g. `type:identity`). */
export function getStringAvatarColor(key: string): AvatarColor {
  const index = key.split('').reduce((acc, c) => acc + c.charCodeAt(0), 0);
  return COLORS[index % COLORS.length];
}

export function getStringAvatarLetter(text: string): string {
  const trimmed = text.trim();
  return trimmed[0]?.toUpperCase() ?? '?';
}
