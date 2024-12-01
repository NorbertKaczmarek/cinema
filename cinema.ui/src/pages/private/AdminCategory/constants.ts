import { PATHS } from 'Api/paths';
import { ROUTES } from 'Routing/routes';

export const CATEGORY_DICT_STATE = {
  paths: {
    getData: PATHS.CATEGORIES.PRIVATE.GET_CATEGORY,
    createData: PATHS.CATEGORIES.PRIVATE.CREATE_CATEGORY,
    updateData: PATHS.CATEGORIES.PRIVATE.UPDATE_CATEGORY,
    deleteData: PATHS.CATEGORIES.PRIVATE.DELETE_CATEGORY,
  },
  initialData: {
    id: '',
    name: '',
  },
  listUrl: ROUTES.private.CATEGORY.TABLE,
  queryKey: (id = 'new') => ['admin', 'dictionary', 'category', id],
};
