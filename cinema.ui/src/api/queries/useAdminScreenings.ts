import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Screening } from 'Types/screening';
import { BackendTable, QueryParamsTable } from 'Types/table';

import { QUERY_KEYS } from './queryKeys';

export const useAdminScreenings = (
  queryParams: QueryParamsTable
): UseQueryResult<BackendTable<Screening>, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.ADMIN_SCREENINGS(queryParams),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.SCREENINGS.PRIVATE.SCREENINGS, {
        params: queryParams,
      });

      return data;
    },
  });
