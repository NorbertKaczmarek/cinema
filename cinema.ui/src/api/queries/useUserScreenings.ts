import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Screening } from 'Types/screening';

import { QUERY_KEYS } from './queryKeys';

export const useUserScreenings = (): UseQueryResult<Screening[], Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_SCREENINGS,
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.SCREENINGS.PUBLIC.SCREENINGS);

      return data;
    },
  });