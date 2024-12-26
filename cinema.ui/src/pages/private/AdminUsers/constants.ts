import { z } from 'zod';

import { PATHS } from 'Api/paths';
import { ROUTES } from 'Routing/routes';

const shared = {
  paths: {
    getData: PATHS.USERS.PRIVATE.GET_USER,
    createData: PATHS.USERS.PRIVATE.CREATE_USER,
    updateData: PATHS.USERS.PRIVATE.UPDATE_USER,
    deleteData: PATHS.USERS.PRIVATE.DELETE_USER,
  },
  initialData: {
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    isAdmin: false,
  },
  listUrl: ROUTES.private.USER.TABLE,
  queryKey: (id = 'new') => ['admin', 'dictionary', 'user', id],
  listQueryKey: ['admin', 'users'],
};

export const USER_ADD_DICT_STATE = {
  ...shared,
  schema: z.object({
    firstName: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
    lastName: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
    email: z
      .string()
      .nonempty('To pole jest wymagane')
      .email('Niepoprawny adres email')
      .max(255, 'Maksymalnie 255 znaków'),
  }),
};

export const USER_DETAILS_DICT_STATE = {
  ...shared,
  schema: z
    .object({
      email: z.string().email('Niepoprawny adres email').max(255, 'Maksymalnie 255 znaków'),
      password: z.string().nonempty('To pole jest wymagane'),
      newPassword: z.string().optional(),
      confirmNewPassword: z.string().optional(),
    })
    .refine(
      data =>
        (!data.newPassword && !data.confirmNewPassword) ||
        (data.newPassword && data.confirmNewPassword),
      {
        message: 'Oba pola są wymagane',
        path: ['confirmNewPassword'],
      }
    ),
};
