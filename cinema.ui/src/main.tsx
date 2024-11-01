import { StrictMode } from 'react';
import { QueryClientProvider } from '@tanstack/react-query';

import { createRoot } from 'react-dom/client';

import queryClient from 'Configs/queryClient';

import './index.css';
import App from './App';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
    </QueryClientProvider>
  </StrictMode>
);
