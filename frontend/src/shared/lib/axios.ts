import axios from 'axios';
import { keycloak } from './keycloak';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? '/api/v1',
});

apiClient.interceptors.request.use(async (config) => {
  if (keycloak.authenticated) {
    await keycloak.updateToken(30).catch(() => keycloak.login());
    config.headers.Authorization = `Bearer ${keycloak.token}`;
  }
  return config;
});
