import axios from 'axios';
import { storage } from '../lib/storage';

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5091';

export const apiClient = axios.create({
  baseURL: apiBaseUrl
});

apiClient.interceptors.request.use((config) => {
  const token = storage.getToken();
  if (token) {
    config.headers = config.headers ?? {};
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      storage.clearToken();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);
