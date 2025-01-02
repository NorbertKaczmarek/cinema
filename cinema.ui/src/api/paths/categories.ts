const PRIVATE = {
  CATEGORIES: '/admin/categories',
  GET_CATEGORY: (id: string) => `/admin/categories/${id}`,
  CREATE_CATEGORY: '/admin/categories',
  UPDATE_CATEGORY: (id: string) => `/admin/categories/${id}`,
  DELETE_CATEGORY: (id: string) => `/admin/categories/${id}`,
};

const PUBLIC = {
  CATEGORIES: '/user/categories',
  GET_CATEGORY: (id: string) => `/user/categories/${id}`,
};

export const CATEGORIES = {
  PRIVATE,
  PUBLIC,
};
