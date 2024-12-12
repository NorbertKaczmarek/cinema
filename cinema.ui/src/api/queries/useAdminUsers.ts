import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { BackendTable, QueryParamsTable } from 'Types/table';
import { User } from 'Types/user';

import { QUERY_KEYS } from './queryKeys';

export const useAdminUsers = (
  queryParams: QueryParamsTable
): UseQueryResult<BackendTable<User>, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.ADMIN_USERS(queryParams),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.USERS.PRIVATE.USERS, {
        params: queryParams,
      });

      return data;
    },
  });
