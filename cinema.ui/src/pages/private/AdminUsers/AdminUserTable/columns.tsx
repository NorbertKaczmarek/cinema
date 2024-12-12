import { ColumnDef } from '@tanstack/react-table';
import { Row } from '@tanstack/table-core/src/types';

import { UserActionCell } from 'Components/Cells';
import { User } from 'Types/user';

export const columns: ColumnDef<User>[] = [
  {
    id: 'name',
    accessorKey: 'name',
    header: 'Pracownik',
    minSize: 200,
    cell: ({ row }: { row: Row<User> }) => (
      <span>{`${row.original.firstName} ${row.original.lastName}`}</span>
    ),
  },
  {
    id: 'email',
    accessorKey: 'email',
    header: 'Adres e-mail',
    minSize: 200,
  },
  {
    id: 'action',
    accessorKey: 'action',
    header: '',
    cell: ({ row }: { row: Row<User> }) => <UserActionCell userId={row.original.id} />,
  },
];
