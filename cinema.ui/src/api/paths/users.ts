const PRIVATE = {
  USERS: '/admin/users',
  GET_USER: (id: string) => `/admin/users/${id}`,
  CREATE_USER: '/admin/users',
  UPDATE_USER: (id: string) => `/admin/users/${id}`,
  DELETE_USER: (id: string) => `/admin/users/${id}`,
  RESET_USER_PASSWORD: (id: string) => `/admin/users/${id}/resetpassword`,
};

const PUBLIC = {};

export const USERS = {
  PRIVATE,
  PUBLIC,
};
