import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from './auth/AuthProvider';
import { AuthGuard } from './auth/AuthGuard';
import { CallbackPage } from './auth/CallbackPage';
import { AppLayout } from './components/layout/AppLayout';
import { RouteErrorFallback } from './components/RouteErrorFallback';
import { ProfilePage } from './pages/profile/ProfilePage';
import { EmailsPage } from './pages/emails/EmailsPage';
import { ConnectionsPage } from './pages/connections/ConnectionsPage';
import { ThemeContextProvider } from './theme/ThemeContext';
import { SnackbarProvider } from './components/SnackbarProvider';
import { ApiClientProvider } from './api/ApiClientProvider';
import { AppErrorBoundary } from './components/AppErrorBoundary';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

const routeErrorElement = <RouteErrorFallback />;

const router = createBrowserRouter([
  {
    path: '/authentication/login-callback',
    element: <CallbackPage />,
    errorElement: routeErrorElement,
  },
  {
    path: '/',
    element: (
      <AuthGuard>
        <ApiClientProvider>
          <AppLayout />
        </ApiClientProvider>
      </AuthGuard>
    ),
    errorElement: routeErrorElement,
    children: [
      { index: true, element: <ProfilePage /> },
      { path: 'emails', element: <EmailsPage /> },
      { path: 'connections', element: <ConnectionsPage /> },
    ],
  },
]);

export default function App() {
  return (
    <AppErrorBoundary>
      <ThemeContextProvider>
        <SnackbarProvider>
          <QueryClientProvider client={queryClient}>
            <AuthProvider>
              <RouterProvider router={router} />
            </AuthProvider>
          </QueryClientProvider>
        </SnackbarProvider>
      </ThemeContextProvider>
    </AppErrorBoundary>
  );
}
