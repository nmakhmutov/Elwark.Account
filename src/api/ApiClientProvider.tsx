/* eslint-disable react-refresh/only-export-components */
import { createContext, useContext, useMemo, type ReactNode } from 'react';
import { useAuth } from 'react-oidc-context';
import { useTranslation } from 'react-i18next';
import type { AxiosInstance } from 'axios';
import { createApiClient } from './client';

const ApiClientContext = createContext<AxiosInstance | null>(null);

export function ApiClientProvider({ children }: { children: ReactNode }) {
  const auth = useAuth();
  const { i18n } = useTranslation();

  const client = useMemo(
    () => createApiClient(() => auth.user?.access_token, i18n.resolvedLanguage ?? i18n.language),
    [auth.user?.access_token, i18n.language, i18n.resolvedLanguage]
  );

  return (
    <ApiClientContext.Provider value={client}>
      {children}
    </ApiClientContext.Provider>
  );
}

export function useApiClient(): AxiosInstance {
  const ctx = useContext(ApiClientContext);
  if (!ctx) throw new Error('useApiClient must be used within ApiClientProvider');
  return ctx;
}
