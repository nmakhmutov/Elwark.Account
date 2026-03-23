import { isAxiosError } from 'axios';
import type { ApiError } from './types';

export function normalizeApiError(err: unknown): ApiError {
  if (isAxiosError(err) && err.response?.data !== undefined) {
    const d = err.response.data;
    const status = err.response.status;

    if (typeof d === 'string') {
      return {
        title: 'Error',
        type: 'unknown',
        detail: d,
        status,
        errors: {},
      };
    }

    if (typeof d === 'object' && d !== null) {
      const obj = d as Record<string, unknown>;
      const rawErrors = obj.errors;
      let errors: Record<string, string[]> = {};
      if (
        rawErrors &&
        typeof rawErrors === 'object' &&
        !Array.isArray(rawErrors)
      ) {
        errors = Object.fromEntries(
          Object.entries(rawErrors).map(([k, v]) => [
            k,
            Array.isArray(v)
              ? v.filter((x): x is string => typeof x === 'string')
              : typeof v === 'string'
                ? [v]
                : [],
          ])
        );
      }

      return {
        title: typeof obj.title === 'string' ? obj.title : 'Error',
        type: typeof obj.type === 'string' ? obj.type : 'unknown',
        detail: typeof obj.detail === 'string' ? obj.detail : null,
        status: typeof obj.status === 'number' ? obj.status : status,
        errors,
      };
    }
  }

  if (err instanceof Error) {
    return {
      title: 'Unexpected error',
      type: 'unknown',
      detail: err.message,
      status: 500,
      errors: {},
    };
  }

  return {
    title: 'Unexpected error',
    type: 'unknown',
    detail: null,
    status: 500,
    errors: {},
  };
}

/** User-facing text: prefer `detail`, then field `errors`, then `title`. */
export function apiErrorMessage(api: ApiError): string {
  if (api.detail?.trim()) return api.detail.trim();

  const parts = Object.values(api.errors)
    .flat()
    .map((s) => (typeof s === 'string' ? s.trim() : ''))
    .filter(Boolean);
  if (parts.length) return parts.join(' ');

  if (api.title?.trim()) return api.title.trim();

  return 'Unexpected error';
}

export function formatApiError(err: unknown): string {
  return apiErrorMessage(normalizeApiError(err));
}

export type ApiErrorSnackbarPayload =
  | string
  | { title?: string; body: string };

/**
 * Snackbar content: separate title (summary) and body (detail / field errors), not one concatenated line.
 * Falls back to a single string when only one piece of text exists.
 */
export function apiErrorSnackbarPayload(err: unknown): ApiErrorSnackbarPayload {
  const api = normalizeApiError(err);
  const title = api.title?.trim() || '';
  const detail = api.detail?.trim() || '';
  const fromErrors = Object.values(api.errors)
    .flat()
    .map((s) => (typeof s === 'string' ? s.trim() : ''))
    .filter(Boolean);

  const bodyLines: string[] = [];
  if (detail) bodyLines.push(detail);
  for (const line of fromErrors) {
    if (line && !bodyLines.includes(line)) bodyLines.push(line);
  }
  const body = bodyLines.join('\n');

  if (!title && !body) {
    return 'Unexpected error';
  }

  if (!title) {
    return { body };
  }

  if (!body) {
    return { body: title };
  }

  if (title === body) {
    return { body: title };
  }

  return { title, body };
}
