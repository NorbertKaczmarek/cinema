import { Row } from '@tanstack/table-core/src/types';

import { RedirectCell } from 'Components/RedirectCell';
import { Movie } from 'Types/movie';

export const columns = [
  {
    id: 'title',
    accessorKey: 'title',
    header: 'Tytuł',
    minSize: 200,
    enableSorting: true,
    cell: ({ row }: { row: Row<Movie> }) => (
      <RedirectCell path={`/admin/movies/${row.getValue('id')}`}>
        {row.getValue('title')}
      </RedirectCell>
    ),
  },
  {
    id: 'duration',
    accessorKey: 'duration',
    header: 'Długość',
    size: 150,
    enableSorting: true,
  },
  {
    id: 'rating',
    accessorKey: 'rating',
    header: 'oceny',
    size: 100,
    enableSorting: true,
  },
  {
    id: 'id',
    accessorKey: 'id',
    header: '',
    size: 1,
    cell: () => null,
  },
];
