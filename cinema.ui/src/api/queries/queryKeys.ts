export const QUERY_KEYS = {
  ADMIN_MOVIES: (params: Record<string, unknown>) => ['admin', 'movies', params],
  ADMIN_CATEGORIES: (params: Record<string, unknown>) => ['admin', 'categories', params],
};
