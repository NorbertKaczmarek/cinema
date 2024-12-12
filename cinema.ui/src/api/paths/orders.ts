const PRIVATE = {
  ORDERS: '/admin/orders',
  GET_ORDER: (id: string) => `/admin/orders/${id}`,
  CREATE_ORDER: '/admin/orders',
  UPDATE_ORDER: (id: string) => `/admin/orders/${id}`,
  DELETE_ORDER: (id: string) => `/admin/orders/${id}`,
};

const PUBLIC = {};

export const ORDERS = {
  PRIVATE,
  PUBLIC,
};
