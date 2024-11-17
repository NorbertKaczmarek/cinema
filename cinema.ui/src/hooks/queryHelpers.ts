import {
  useMutation,
  UseMutationOptions,
  UseMutationResult,
  useQuery,
  UseQueryOptions,
  UseQueryResult,
} from '@tanstack/react-query';

import { AxiosError } from 'axios';

import { httpClient } from 'Configs/axios';

const GET = <T>(
  url: (id: string) => string,
  id: string | undefined,
  queryKey: (string | number | undefined)[],
  hookOptions?: UseQueryOptions<T, AxiosError>
): UseQueryResult<T> => {
  const axios = httpClient();

  return useQuery<T, AxiosError>(
    queryKey,
    async () => {
      const { data } = await axios.get(url(id as string));

      return data;
    },
    { enabled: !!id, ...hookOptions }
  );
};

const POST = <T, P>(
  url: string,
  hookOptions?: UseMutationOptions<T, AxiosError, P>
): UseMutationResult<T, AxiosError, P> => {
  const axios = httpClient();
  return useMutation<T, AxiosError, P>(async updatedData => {
    const { data } = await axios.post(url, updatedData);

    return { ...updatedData, ...data };
  }, hookOptions);
};

const PUT = <T, P>(
  url: (id: string) => string,
  id: string | undefined,
  hookOptions?: UseMutationOptions<T, AxiosError, P>
): UseMutationResult<T, AxiosError, P> => {
  const axios = httpClient();
  return useMutation<T, AxiosError, P>(async updatedData => {
    const { data } = await axios.put(url(id as string), updatedData);

    return { ...updatedData, ...data };
  }, hookOptions);
};
const DELETE = (
  url: (id: string) => string,
  id: string | undefined,
  hookOptions?: UseMutationOptions<void, AxiosError>
): UseMutationResult<void, AxiosError, void> => {
  const axios = httpClient();
  return useMutation<void, AxiosError, void>(async variables => {
    const { data } = await axios.delete(url(id as string), {
      data: variables,
    });

    return data;
  }, hookOptions);
};

export const queryHelpers = {
  GET,
  POST,
  PUT,
  DELETE,
};
