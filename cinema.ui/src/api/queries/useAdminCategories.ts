import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Category } from 'Types/category.ts';
import { BackendTable, QueryParamsTable } from 'Types/table';

import { QUERY_KEYS } from './queryKeys.ts';

export const useAdminCategories = (
  queryParams: QueryParamsTable
): UseQueryResult<Category[], Error> =>
  useQuery({
    queryKey: QUERY_KEYS.ADMIN_CATEGORIES(queryParams),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.CATEGORIES.PRIVATE.CATEGORIES, {
        params: queryParams,
      });

      return data;
    },
  });
