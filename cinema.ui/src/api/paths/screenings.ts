const PRIVATE = {
  SCREENINGS: '/admin/screenings',
  GET_SCREENING: (id: string) => `/admin/screenings/${id}`,
  CREATE_SCREENING: '/admin/screenings',
  UPDATE_SCREENING: (id: string) => `/admin/screenings/${id}`,
  DELETE_SCREENING: (id: string) => `/admin/screenings/${id}`,
};

const PUBLIC = {};

export const SCREENINGS = {
  PRIVATE,
  PUBLIC,
};
