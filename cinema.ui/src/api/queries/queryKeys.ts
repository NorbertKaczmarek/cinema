export const QUERY_KEYS = {
  ADMIN_MOVIES: (params: Record<string, unknown>) => ['admin', 'movies', params],
  ADMIN_CATEGORIES: (params: Record<string, unknown>) => ['admin', 'categories', params],
  ADMIN_SCREENINGS: (params: Record<string, unknown>) => ['admin', 'screenings', params],
  ADMIN_USERS: (params: Record<string, unknown>) => ['admin', 'users', params],
  ADMIN_ORDERS: (params: Record<string, unknown>) => ['admin', 'orders', params],
};
