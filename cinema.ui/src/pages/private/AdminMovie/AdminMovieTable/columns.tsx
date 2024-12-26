import { Row } from '@tanstack/table-core/src/types';

import { RedirectCell } from 'Components/Cells';
import { Movie } from 'Types/movie';
import { formatDuration } from 'Utils/formatDuration';

export const columns = [
  {
    id: 'title',
    accessorKey: 'title',
    header: 'Tytuł',
    minSize: 200,
    enableSorting: true,
    cell: ({ row }: { row: Row<Movie> }) => (
      <RedirectCell route={`/admin/movies/${row.original.id}`}>
        {row.getValue('title')}
      </RedirectCell>
    ),
  },
  {
    id: 'durationMinutes',
    accessorKey: 'durationMinutes',
    header: 'Długość',
    size: 150,
    enableSorting: true,
    cell: ({ row }: { row: Row<Movie> }) => (
      <span>{formatDuration(row.getValue('durationMinutes'))}</span>
    ),
  },
  {
    id: 'rating',
    accessorKey: 'rating',
    header: 'Ocena',
    size: 100,
    enableSorting: true,
  },
];
