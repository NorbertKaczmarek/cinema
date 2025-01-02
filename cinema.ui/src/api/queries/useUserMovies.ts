import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Movie } from 'Types/movie';

import { QUERY_KEYS } from './queryKeys';

export const useUserMovies = (): UseQueryResult<Movie[], Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_MOVIES,
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.MOVIES.PUBLIC.MOVIES);

      return data;
    },
  });
