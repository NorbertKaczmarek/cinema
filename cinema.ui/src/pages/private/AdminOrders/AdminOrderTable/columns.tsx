import { ColumnDef } from '@tanstack/react-table';
import { Row } from '@tanstack/table-core/src/types';

import {
  OrderActionCell,
  OrderStatusCell,
  ScreeningDateCell,
  ScreeningMovieCell,
} from 'Components/Cells';
import { Order } from 'Types/order';

export const columns: ColumnDef<Order>[] = [
  {
    id: 'email',
    accessorKey: 'email',
    header: 'Adres e-mail',
    minSize: 100,
  },
  {
    id: 'phoneNumber',
    accessorKey: 'phoneNumber',
    header: 'Numer telefonu',
    size: 100,
  },
  {
    id: 'movieName',
    accessorKey: 'movieName',
    header: 'Film',
    minSize: 100,
    cell: ({ row }: { row: Row<Order> }) => (
      <ScreeningMovieCell screeningId={row.original.screeningId} />
    ),
  },
  {
    id: 'date',
    accessorKey: 'date',
    header: 'Data seansu',
    minSize: 100,
    cell: ({ row }: { row: Row<Order> }) => (
      <ScreeningDateCell screeningId={row.original.screeningId} />
    ),
  },
  {
    id: 'status',
    accessorKey: 'status',
    header: 'Status',
    minSize: 50,
    cell: ({ row }: { row: Row<Order> }) => <OrderStatusCell status={row.original.status} />,
  },
  {
    id: 'action',
    accessorKey: 'action',
    header: '',
    minSize: 50,
    cell: ({ row }: { row: Row<Order> }) => (
      <OrderActionCell orderId={row.original.id} orderStatus={row.original.status} />
    ),
  },
];
