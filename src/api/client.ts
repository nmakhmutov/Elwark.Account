import axios, { type AxiosInstance } from 'axios';

export function createApiClient(
  getAccessToken: () => string | undefined,
  locale: string
): AxiosInstance {
  const client = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
  });

  client.interceptors.request.use((config) => {
    const token = getAccessToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    config.headers['Accept-Language'] = locale;
    return config;
  });

  return client;
}
