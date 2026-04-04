import { useAuth } from 'react-oidc-context';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import { LoadingScreen } from '../components/LoadingScreen';
import { ErrorScreen } from '../components/ErrorScreen';

export function CallbackPage() {
  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!auth.isLoading && auth.isAuthenticated) {
      navigate('/', { replace: true });
    }
  }, [auth.isLoading, auth.isAuthenticated, navigate]);

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

  return <LoadingScreen />;
}
