import { PATHS } from 'Api/paths';
import { ROUTES } from 'Routing/routes';

export const SCREENING_DICT_STATE = {
  paths: {
    getData: PATHS.SCREENINGS.PRIVATE.GET_SCREENING,
    createData: PATHS.SCREENINGS.PRIVATE.CREATE_SCREENING,
    updateData: PATHS.SCREENINGS.PRIVATE.UPDATE_SCREENING,
    deleteData: PATHS.SCREENINGS.PRIVATE.DELETE_SCREENING,
  },
  initialData: {
    id: '',
    movieId: '',
    startDateTime: '',
    endDateTime: '',
  },
  listUrl: ROUTES.private.SCREENING.TABLE,
  queryKey: (id = 'new') => ['admin', 'dictionary', 'screening', id],
  listQueryKey: ['admin', 'screenings'],
};
