import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Movie } from 'Types/movie';

import { QUERY_KEYS } from './queryKeys';

export const useUserMovieById = (id?: string): UseQueryResult<Movie, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_MOVIE(id),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.MOVIES.PUBLIC.GET_MOVIE(id as string));

      return data;
    },
    enabled: !!id,
  });
