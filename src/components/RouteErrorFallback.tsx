import { isRouteErrorResponse, useNavigate, useRouteError } from 'react-router-dom';
import { ErrorScreen } from './ErrorScreen';

/**
 * Shown when a route throws (render, loader, action). Wired via `errorElement` on the router.
 */
export function RouteErrorFallback() {
  const error = useRouteError();
  const navigate = useNavigate();

  let title = 'Something went wrong';
  let message = 'An unexpected error occurred. You can try reloading the page or going back home.';
  let details = '';

  if (isRouteErrorResponse(error)) {
    title = `${error.status} ${error.statusText || 'Error'}`.trim();
    const data = error.data;
    message =
      typeof data === 'string'
        ? data
        : data != null && typeof data === 'object' && 'message' in data
          ? String((data as { message: unknown }).message)
          : error.statusText || message;
  } else if (error instanceof Error) {
    message = error.message;
    details = import.meta.env.DEV ? (error.stack ?? '') : '';
  } else {
    message = String(error);
  }

  return (
    <ErrorScreen
      title={title}
      message={message}
      details={import.meta.env.DEV ? details : null}
      actionLabel="Go to home"
      onAction={() => navigate('/', { replace: true })}
    />
  );
}
