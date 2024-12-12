import { PATHS } from 'Api/paths';
import { ROUTES } from 'Routing/routes';

export const USER_DICT_STATE = {
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
