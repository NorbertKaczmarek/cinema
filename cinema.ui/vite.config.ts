import react from '@vitejs/plugin-react';

import { resolve } from 'path';
import { defineConfig } from 'vite';

export default defineConfig({
  plugins: [react()],
  base: '/',
  resolve: {
    alias: {
      Components: resolve(__dirname, './src/components/'),
      Configs: resolve(__dirname, 'src/configs/'),
      Hooks: resolve(__dirname, './src/hooks/'),
      Types: resolve(__dirname, './src/types/'),
      Utils: resolve(__dirname, './src/utils/'),
      Constants: resolve(__dirname, './src/constants/'),
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
