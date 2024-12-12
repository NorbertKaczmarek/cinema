import { useQuery, UseQueryResult } from '@tanstack/react-query';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';
import { Order } from 'Types/order';
import { BackendTable, QueryParamsTable } from 'Types/table';

import { QUERY_KEYS } from './queryKeys';

export const useAdminOrders = (
  queryParams: QueryParamsTable
): UseQueryResult<BackendTable<Order>, Error> =>
  useQuery({
    queryKey: QUERY_KEYS.ADMIN_ORDERS(queryParams),
    queryFn: async () => {
      const axios = httpClient();
      const { data } = await axios.get(PATHS.ORDERS.PRIVATE.ORDERS, {
        params: queryParams,
      });

      return data;
    },
  });
