import { PaginationState } from '@tanstack/react-table';

export type BackendTable<T> = {
  content: T[];
  totalElements: number;
  totalPages: number;
};

export type QueryParamsTable = {
  page: number;
  size: number;
  phrase?: string;
};

export type ParamsState = {
  phrase?: string;
  pagination: PaginationState;
};
