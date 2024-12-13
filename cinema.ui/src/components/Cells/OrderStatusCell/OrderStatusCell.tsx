import { OrderStatus } from 'Types/order';

interface Props {
  status: OrderStatus;
}

const statusMap = {
  [OrderStatus.READY]: {
    title: 'Opłacony',
    className: 'text-success font-bold',
  },
  [OrderStatus.PENDING]: {
    title: 'Oczekujący',
    className: 'text-warning font-bold',
  },
  [OrderStatus.CANCELLED]: {
    title: 'Anulowany',
    className: 'text-destructive font-bold',
  },
} as const;

export const OrderStatusCell = ({ status }: Props) => {
  // @TODO classname WITH color
  return <span className={statusMap[status].className}>{statusMap[status].title ?? ''}</span>;
};
