import { OrderStatus } from 'Types/order';

interface Props {
  status: OrderStatus;
}

const statusMap = {
  [OrderStatus.READY]: {
    title: 'Opłacony',
  },
  [OrderStatus.PENDING]: {
    title: 'Oczekujący',
  },
  [OrderStatus.CANCELLED]: {
    title: 'Anulowany',
  },
} as const;

export const OrderStatusCell = ({ status }: Props) => {
  // @TODO classname WITH color
  return <span>{statusMap[status].title ?? ''}</span>;
};
