import { toast } from 'sonner';

import { PATHS } from 'Api/paths';
import { useAdminResetPassword } from 'Api/queries/useAdminResetPassword';
import { Button, ButtonVariant } from 'Components/Button';
import queryClient from 'Configs/queryClient';
import { queryHelpers } from 'Hooks/queryHelpers';

interface Props {
  userId: string;
}

export const UserActionCell = ({ userId }: Props) => {
  const { mutate: deleteUser, isLoading: isLoadingDelete } = queryHelpers.DELETE(
    PATHS.USERS.PRIVATE.DELETE_USER,
    userId,
    {
      onSuccess: async () => {
        toast.success('Pomyślnie usunięto element');
        await queryClient.invalidateQueries(['admin', 'users']);
      },
    }
  );

  const { mutate: resetPassword, isLoading: isLoadingReset } = useAdminResetPassword(userId, {
    onSuccess: async () => {
      toast.success('Pomyślnie zresetowano hasło');
    },
  });

  const isSpinning = isLoadingReset || isLoadingDelete;

  return (
    <div className="flex justify-end gap-4">
      <Button
        className="min-w-[120px]"
        variant={ButtonVariant.Outline}
        isLoading={isSpinning}
        onClick={() => resetPassword()}
      >
        Resetuj hasło
      </Button>
      <Button
        className="min-w-[40px]"
        variant={ButtonVariant.Danger}
        isLoading={isSpinning}
        onClick={() => deleteUser()}
      >
        Usuń
      </Button>
    </div>
  );
};
