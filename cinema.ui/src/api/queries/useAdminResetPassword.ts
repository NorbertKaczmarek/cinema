import { useMutation, UseMutationOptions, UseMutationResult } from '@tanstack/react-query';

import { AxiosError } from 'axios';

import { PATHS } from 'Api/paths';
import { httpClient } from 'Configs/axios';

export const useAdminResetPassword = (
  userId: string,
  hookOptions?: UseMutationOptions<void, AxiosError>
): UseMutationResult<void, AxiosError, void> => {
  const axios = httpClient();
  return useMutation<void, AxiosError, void>(async () => {
    const { data } = await axios.post(PATHS.USERS.PRIVATE.RESET_USER_PASSWORD(userId));

    return data;
  }, hookOptions);
};
