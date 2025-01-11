import { z } from 'zod';

import { PATHS } from 'Api/paths';
import { ROUTES } from 'Routing/routes';
import { getYouTubeId } from 'Utils/getYoutubeId';

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
    backgroundUrl: '',
    trailerUrl: '',
    rating: 0,
    durationMinutes: 0,
    title: '',
  },
  listUrl: ROUTES.private.MOVIE.TABLE,
  queryKey: (id = 'new') => ['admin', 'dictionary', 'movie', id],
  listQueryKey: ['admin', 'movies'],
  schema: z.object({
    id: z.string(),
    cast: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
    categoryId: z.string().nonempty('To pole jest wymagane'),
    description: z.string().nonempty('To pole jest wymagane').max(5000, 'Maksymalnie 5000 znaków'),
    director: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
    posterUrl: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
    backgroundUrl: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
    trailerUrl: z
      .string()
      .nonempty('To pole jest wymagane')
      .refine(url => getYouTubeId(url) !== null, {
        message: 'Niepoprawny format URL',
      }),
    rating: z.coerce.number().min(0.1, 'Minimalna wartość: 0.1').max(10, 'Maksymalna wartość: 10'),
    durationMinutes: z.coerce.number().min(1, 'Minimalna wartość: 1').int(),
    title: z.string().nonempty('To pole jest wymagane').max(255, 'Maksymalnie 255 znaków'),
  }),
};
