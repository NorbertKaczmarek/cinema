import qs from 'qs';

import axios, { AxiosInstance, InternalAxiosRequestConfig } from 'axios';

export const BACKEND_URL = import.meta.env.VITE_API_URL;

export const httpClient = (): AxiosInstance => {
  const httpClient = axios.create({
    baseURL: 'https://develop.cinema.nkaczmarek.pl/api',
    headers: {
      'Access-Control-Allow-Origin': '*',
    },
  });

  httpClient.defaults.paramsSerializer = params =>
    qs.stringify(params, { arrayFormat: 'repeat', encode: false });

  httpClient.interceptors.request.use(config => {
    const authHeader = sessionStorage.getItem('token') || '';

    return {
      ...config,
      headers: {
        ...config.headers,
        Authorization: authHeader,
      },
    } as InternalAxiosRequestConfig;
  });

  httpClient.interceptors.response.use(
    res => res,
    err => {
      if (!err.response) {
        return Promise.reject(err);
      }

      switch (err.response.status) {
        case 500:
        case 502:
        case 503:
        case 504:
          return Promise.reject({
            message: 'Wystąpił błąd serwera',
            code: '500',
          });
        case 404:
          window.location.href = '/not-found';
          return Promise.reject(err);
        case 403: {
          window.location.href = '/forbidden';
          return Promise.reject(err);
        }
        case 409:
        case 406:
        case 400:
          return Promise.reject(err);
        case 401: {
          sessionStorage.removeItem('token');
          window.location.href = '/login';
          return Promise.reject(err);
        }
        case undefined:
          window.history.pushState('', '', '/not-found');
          return Promise.reject(err);
        default:
          return Promise.reject(err);
      }
    }
  );
  return httpClient;
};
