import { ColumnDef } from '@tanstack/react-table';
import { Row } from '@tanstack/table-core/src/types';

import { RedirectCell } from 'Components/Cells';
import { Category } from 'Types/category';

export const columns: ColumnDef<Category>[] = [
  {
    id: 'name',
    accessorKey: 'name',
    header: 'Nazwa kategorii',
    minSize: 200,
    enableSorting: true,
    cell: ({ row }: { row: Row<Category> }) => (
      <RedirectCell route={`/admin/categories/${row.original.id}`}>
        {row.getValue('name')}
      </RedirectCell>
    ),
  },
];
