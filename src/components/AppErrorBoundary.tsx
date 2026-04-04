import { Component, type ErrorInfo, type ReactNode } from 'react';
import { ErrorScreen } from './ErrorScreen';

interface AppErrorBoundaryProps {
  children: ReactNode;
}

interface AppErrorBoundaryState {
  error: Error | null;
  errorInfo: ErrorInfo | null;
}

export class AppErrorBoundary extends Component<
  AppErrorBoundaryProps,
  AppErrorBoundaryState
> {
  state: AppErrorBoundaryState = {
    error: null,
    errorInfo: null,
  };

  static getDerivedStateFromError(error: Error): AppErrorBoundaryState {
    return {
      error,
      errorInfo: null,
    };
  }

  override componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    this.setState({ error, errorInfo });
  }

  private handleReload = () => {
    window.location.reload();
  };

  override render() {
    const { error, errorInfo } = this.state;

    if (error) {
      return (
        <ErrorScreen
          title="Application error"
          message={error.message || 'An unexpected error occurred.'}
          details={import.meta.env.DEV ? errorInfo?.componentStack ?? error.stack ?? null : null}
          actionLabel="Reload page"
          onAction={this.handleReload}
        />
      );
    }

    return this.props.children;
  }
}
