import { parse } from 'tldts';

export type EmailProviderKind =
  | 'google'
  | 'microsoft'
  | 'yahoo'
  | 'apple'
  | 'proton'
  | 'other';

/** Lowercase host from `user@host`, or empty if invalid. */
export function getEmailDomain(email: string): string {
  const at = email.lastIndexOf('@');
  if (at < 0 || at === email.length - 1) return '';
  return email.slice(at + 1).toLowerCase().trim();
}

/**
 * First label of the registrable domain (eTLD+1), e.g. `gmail.com` → `gmail`,
 * `outlook.co.uk` → `outlook`, `mail.yahoo.com` → `yahoo`. Uses the public suffix list via `tldts`.
 */
export function getEmailRegistrableKey(domain: string): string {
  const d = domain.toLowerCase().trim();
  if (!d) return '';

  const parsed = parse(d, { allowPrivateDomains: false });
  const registrable = parsed.domain;
  if (!registrable) {
    const labels = d.split('.').filter(Boolean);
    return labels[0] ?? '';
  }

  return registrable.split('.')[0] ?? '';
}

const GOOGLE_KEYS = new Set(['gmail', 'googlemail', 'google']);
const MICROSOFT_KEYS = new Set(['outlook', 'hotmail', 'live', 'msn', 'microsoft', 'office365', 'onmicrosoft']);
const YAHOO_KEYS = new Set(['yahoo', 'ymail', 'rocketmail']);
const APPLE_KEYS = new Set(['icloud', 'me', 'mac', 'apple']);
const PROTON_KEYS = new Set(['proton', 'protonmail', 'pm']);

export function getEmailProviderKind(email: string): EmailProviderKind {
  const domain = getEmailDomain(email);
  if (!domain) return 'other';

  const key = getEmailRegistrableKey(domain);
  if (!key) return 'other';

  if (GOOGLE_KEYS.has(key)) return 'google';
  if (MICROSOFT_KEYS.has(key)) return 'microsoft';
  if (YAHOO_KEYS.has(key)) return 'yahoo';
  if (APPLE_KEYS.has(key)) return 'apple';
  if (PROTON_KEYS.has(key)) return 'proton';

  return 'other';
}
