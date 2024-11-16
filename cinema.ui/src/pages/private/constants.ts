import { PATHS } from 'Api/paths';
import { ROUTES } from 'Routing/routes';

export const MOVIE_DICT_STATE = {
  paths: {
    getData: PATHS.MOVIES.PRIVATE.GET_MOVIE,
    createData: PATHS.MOVIES.PRIVATE.CREATE_MOVIE,
    updateData: PATHS.MOVIES.PRIVATE.UPDATE_MOVIE,
    deleteData: PATHS.MOVIES.PRIVATE.DELETE_MOVIE,
  },
  initialData: {
    id: '',
    cast: '',
    categoryId: '',
    description: '',
    director: '',
    posterUrl: '',
    rating: 0,
    durationMinutes: 0,
    title: '',
  },
  listUrl: ROUTES.private.MOVIES_TABLE,
  queryKey: (id = 'new') => ['admin', 'dictionary', 'movie', id],
};
