import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Screening } from 'Types/screening';

import { QUERY_KEYS } from './queryKeys';

export const useUserScreeningById = (id?: string): UseQueryResult<Screening, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_SCREENING(id),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.SCREENINGS.PUBLIC.GET_SCREENING(id as string));

      return data;
    },
    enabled: !!id,
  });
