import { useState } from 'react';
import { AppBar, Box, Drawer, IconButton, Toolbar } from '@mui/material';
import { Menu as MenuIcon } from '@mui/icons-material';
import { Outlet } from 'react-router-dom';
import { useAccount } from '../../api/hooks/useAccount';
import { Sidebar } from './Sidebar';
import { AppBrandMark } from './AppBrandMark';
import { LanguageMenuControl } from './LanguageMenuControl';
import { LoadingScreen } from '../LoadingScreen';
import { ErrorScreen } from '../ErrorScreen';
import { formatApiError } from '../../api/apiError';

const SIDEBAR_WIDTH = 240;
const TOPBAR_HEIGHT = 56;

export function AppLayout() {
  const [mobileOpen, setMobileOpen] = useState(false);
  const { isLoading, isError, error, data: account, refetch } = useAccount();

  if (isLoading) {
    return <LoadingScreen />;
  }

  if (isError) {
    return (
      <ErrorScreen
        title="Unable to load account"
        message={formatApiError(error)}
        actionLabel="Try again"
        onAction={() => void refetch()}
      />
    );
  }

  if (!account) {
    return (
      <ErrorScreen
        title="Unable to load account"
        message="The account response was empty."
        actionLabel="Reload page"
        onAction={() => window.location.reload()}
      />
    );
  }

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh' }}>
      {/* Desktop sidebar — permanent */}
      <Drawer
        variant="permanent"
        sx={{
          width: SIDEBAR_WIDTH,
          flexShrink: 0,
          display: { xs: 'none', md: 'block' },
          '& .MuiDrawer-paper': {
            width: SIDEBAR_WIDTH,
            boxSizing: 'border-box',
            border: 'none',
            borderRight: '1px solid',
            borderColor: 'divider',
            overflowX: 'hidden',
          },
        }}
      >
        <Sidebar />
      </Drawer>

      {/* Mobile sidebar — temporary */}
      <Drawer
        variant="temporary"
        open={mobileOpen}
        onClose={() => setMobileOpen(false)}
        ModalProps={{ keepMounted: true }}
        sx={{
          display: { xs: 'block', md: 'none' },
          '& .MuiDrawer-paper': {
            width: SIDEBAR_WIDTH,
            maxWidth: '100%',
            boxSizing: 'border-box',
            overflow: 'hidden',
          },
        }}
      >
        <Sidebar onClose={() => setMobileOpen(false)} />
      </Drawer>

      {/* Main content area */}
      <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column', minWidth: 0 }}>
        {/* Mobile topbar */}
        <AppBar
          position="sticky"
          elevation={0}
          sx={{
            display: { xs: 'flex', md: 'none' },
            bgcolor: 'background.paper',
            borderBottom: '1px solid',
            borderColor: 'divider',
            color: 'text.primary',
          }}
        >
          <Toolbar
            sx={{
              height: TOPBAR_HEIGHT,
              display: 'flex',
              alignItems: 'center',
              gap: 1,
              minWidth: 0,
            }}
          >
            <IconButton
              onClick={() => setMobileOpen(true)}
              edge="start"
              sx={{ mr: 0.5 }}
            >
              <MenuIcon />
            </IconButton>
            <Box sx={{ flex: 1, minWidth: 0 }}>
              <AppBrandMark logoHeight={28} titleVariant="subtitle1" />
            </Box>
            <LanguageMenuControl size="medium" />
          </Toolbar>
        </AppBar>

        {/* Page content */}
        <Box
          component="main"
          sx={{
            flex: 1,
            p: { xs: 2, sm: 3, md: 4 },
            maxWidth: 800,
            width: '100%',
            mx: 'auto',
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
}
