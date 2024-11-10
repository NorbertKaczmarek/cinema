import { QueryClientProvider } from '@tanstack/react-query';

import { RouterProvider } from 'react-router-dom';

import { Toaster } from 'Components/Toaster';
import queryClient from 'Configs/queryClient';
import { SidebarProvider } from 'Hooks/useSidebarContext';
import { router } from 'Routing/index';

const App = () => (
  <SidebarProvider>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
      <Toaster />
    </QueryClientProvider>
  </SidebarProvider>
);

export default App;
