import react from '@vitejs/plugin-react';

import { resolve } from 'path';
import { defineConfig } from 'vite';

export default defineConfig({
  plugins: [react()],
  base: '/',
  resolve: {
    alias: {
      Api: resolve(__dirname, './src/api/'),
      Components: resolve(__dirname, './src/components/'),
      Constants: resolve(__dirname, './src/constants/'),
      Configs: resolve(__dirname, 'src/configs/'),
      Hooks: resolve(__dirname, './src/hooks/'),
      Pages: resolve(__dirname, './src/pages/'),
      Routing: resolve(__dirname, './src/routing/'),
      Types: resolve(__dirname, './src/types/'),
      Utils: resolve(__dirname, './src/utils/'),
    },
  },
  server: {
    open: true,
    port: 3000,
  },
  preview: {
    port: 8080,
  },
  build: {
    rollupOptions: {
      input: {
        main: resolve(__dirname, 'index.html'),
      },
    },
  },
});
