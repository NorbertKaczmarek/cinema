import { toast } from 'sonner';

import { PATHS } from 'Api/paths';
import { Button, ButtonVariant } from 'Components/Button';
import queryClient from 'Configs/queryClient';
import { queryHelpers } from 'Hooks/queryHelpers';
import { OrderStatus } from 'Types/order';

interface Props {
  orderId: string;
  orderStatus: OrderStatus;
}

export const OrderActionCell = ({ orderId, orderStatus }: Props) => {
  const { mutate: updateOrder, isLoading: isLoadingUpdate } = queryHelpers.PUT(
    PATHS.ORDERS.PRIVATE.UPDATE_ORDER,
    orderId,
    {
      onSuccess: async () => {
        toast.success('Pomyślnie zaaktualizowano zlecenie');
        await queryClient.invalidateQueries(['admin', 'orders']);
      },
    }
  );

  return (
    <div className="flex justify-end gap-4">
      <Button
        className="min-w-[60px]"
        variant={ButtonVariant.Success}
        isLoading={isLoadingUpdate}
        isDisabled={orderStatus === OrderStatus.READY}
        onClick={() => updateOrder({ status: OrderStatus.READY })}
      >
        Opłać
      </Button>
      <Button
        className="min-w-[60px]"
        variant={ButtonVariant.Danger}
        isLoading={isLoadingUpdate}
        isDisabled={orderStatus === OrderStatus.CANCELLED}
        onClick={() => updateOrder({ status: OrderStatus.CANCELLED })}
      >
        Anuluj
      </Button>
    </div>
  );
};
