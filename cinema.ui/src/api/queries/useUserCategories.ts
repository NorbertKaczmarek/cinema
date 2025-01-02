import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Category } from 'Types/category';
import { BackendTable, QueryParamsTable } from 'Types/table';

import { QUERY_KEYS } from './queryKeys';

export const useUserCategories = (
  queryParams: QueryParamsTable
): UseQueryResult<BackendTable<Category>, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_CATEGORIES(queryParams),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.CATEGORIES.PUBLIC.CATEGORIES, {
        params: queryParams,
      });

      return data;
    },
  });
