import { useAuth } from 'react-oidc-context';
import { useEffect, type ReactNode } from 'react';
import { LoadingScreen } from '../components/LoadingScreen';
import { ErrorScreen } from '../components/ErrorScreen';

export function AuthGuard({ children }: { children: ReactNode }) {
  const auth = useAuth();

  useEffect(() => {
    if (!auth.isLoading && !auth.isAuthenticated) {
      auth.signinRedirect();
    }
  }, [auth.isLoading, auth.isAuthenticated, auth]);

  if (auth.isLoading) {
    return <LoadingScreen />;
  }

  if (auth.error) {
    return (
      <ErrorScreen
        title="Authentication failed"
        message={auth.error.message}
        actionLabel="Reload page"
        onAction={() => window.location.reload()}
      />
    );
  }

  if (!auth.isAuthenticated) {
    return null;
  }

  return <>{children}</>;
}
