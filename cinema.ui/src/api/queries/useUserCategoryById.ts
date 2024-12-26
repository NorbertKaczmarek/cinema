import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Category } from 'Types/category';

import { QUERY_KEYS } from './queryKeys';

export const useUserCategoryById = (id?: string): UseQueryResult<Category, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.USER_CATEGORY(id),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.CATEGORIES.PUBLIC.GET_CATEGORY(id as string));

      return data;
    },
    enabled: !!id,
  });
