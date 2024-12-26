import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { SeatAvailability } from 'Types/screening';

import { QUERY_KEYS } from './queryKeys';

export const useUserScreeningSeats = (
  screeningId?: string
): UseQueryResult<SeatAvailability, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_SCREENING_SEATS(screeningId),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(
        PATHS.SCREENINGS.PUBLIC.SCREENING_SEATS(screeningId as string)
      );

      return data;
    },
    enabled: !!screeningId,
  });
