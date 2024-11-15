import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Movie } from 'Types/movie';
import { BackendTable, QueryParamsTable } from 'Types/table';

import { QUERY_KEYS } from './queryKeys';

export const useAdminMovies = (
  queryParams: QueryParamsTable
): UseQueryResult<BackendTable<Movie>, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.ADMIN_MOVIES(queryParams),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.MOVIES.PRIVATE.MOVIES, {
        params: queryParams,
      });

      return data;
    },
  });
