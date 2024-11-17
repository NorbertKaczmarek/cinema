const PRIVATE = {
  CATEGORIES: '/admin/categories',
  GET_CATEGORY: (id: string) => `/admin/categories/${id}`,
  CREATE_CATEGORY: '/admin/categories',
  UPDATE_CATEGORY: (id: string) => `/admin/categories/${id}`,
  DELETE_CATEGORY: (id: string) => `/admin/categories/${id}`,
};

const PUBLIC = {};

export const CATEGORIES = {
  PRIVATE,
  PUBLIC,
};
