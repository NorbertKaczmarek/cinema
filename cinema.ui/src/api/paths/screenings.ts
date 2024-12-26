const PRIVATE = {
  SCREENINGS: '/admin/screenings',
  GET_SCREENING: (id: string) => `/admin/screenings/${id}`,
  CREATE_SCREENING: '/admin/screenings',
  UPDATE_SCREENING: (id: string) => `/admin/screenings/${id}`,
  DELETE_SCREENING: (id: string) => `/admin/screenings/${id}`,
  SCREENING_SEATS: (id: string) => `/admin/screenings/${id}/seats`,
};

const PUBLIC = {
  SCREENINGS: '/user/screenings',
  SCREENING_SEATS: (id: string) => `/user/screenings/${id}/seats`,
  GET_SCREENING: (id: string) => `/admin/screenings/${id}`,
};

export const SCREENINGS = {
  PRIVATE,
  PUBLIC,
};
