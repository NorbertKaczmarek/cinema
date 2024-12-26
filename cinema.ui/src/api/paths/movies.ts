const PRIVATE = {
  MOVIES: '/admin/movies',
  GET_MOVIE: (id: string) => `/admin/movies/${id}`,
  CREATE_MOVIE: '/admin/movies',
  UPDATE_MOVIE: (id: string) => `/admin/movies/${id}`,
  DELETE_MOVIE: (id: string) => `/admin/movies/${id}`,
};

const PUBLIC = {
  MOVIES: '/user/movies',
  GET_MOVIE: (id: string) => `/user/movies/${id}`,
};

export const MOVIES = {
  PRIVATE,
  PUBLIC,
};
