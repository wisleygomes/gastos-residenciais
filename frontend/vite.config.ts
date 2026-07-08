import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// Configuração padrão do Vite para um projeto React + TypeScript.
// O front-end consome a API do back-end em http://localhost:5000 (ver src/api/api.ts).
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
  },
});
