import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from './auth/AuthProvider';
import { AuthGuard } from './auth/AuthGuard';
import { CallbackPage } from './auth/CallbackPage';
import { AppLayout } from './components/layout/AppLayout';
import { ProfilePage } from './pages/profile/ProfilePage';
import { EmailsPage } from './pages/emails/EmailsPage';
import { ConnectionsPage } from './pages/connections/ConnectionsPage';
import { ThemeContextProvider } from './theme/ThemeContext';
import { SnackbarProvider } from './components/SnackbarProvider';
import { ApiClientProvider } from './api/ApiClientProvider';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

const router = createBrowserRouter([
  {
    path: '/authentication/login-callback',
    element: <CallbackPage />,
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
    children: [
      { index: true, element: <ProfilePage /> },
      { path: 'emails', element: <EmailsPage /> },
      { path: 'connections', element: <ConnectionsPage /> },
    ],
  },
]);

export default function App() {
  return (
    <ThemeContextProvider>
      <SnackbarProvider>
        <QueryClientProvider client={queryClient}>
          <AuthProvider>
            <RouterProvider router={router} />
          </AuthProvider>
        </QueryClientProvider>
      </SnackbarProvider>
    </ThemeContextProvider>
  );
}
