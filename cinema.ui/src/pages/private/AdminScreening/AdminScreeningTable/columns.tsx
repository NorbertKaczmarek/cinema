import { ColumnDef } from '@tanstack/react-table';
import { Row } from '@tanstack/table-core/src/types';

import { MovieCell } from 'Components/Cells';
import { Screening } from 'Types/screening';
import { formatDateTime } from 'Utils/formatDateTime';

export const columns: ColumnDef<Screening>[] = [
  {
    id: 'id',
    accessorKey: 'id',
    header: 'Nazwa filmu',
    minSize: 200,
    cell: ({ row }: { row: Row<Screening> }) => (
      <MovieCell route={`/admin/screenings/${row.getValue('id')}`} movieId={row.original.movieId} />
    ),
  },
  {
    id: 'date',
    accessorKey: 'date',
    header: 'Data seansu',
    minSize: 50,
    cell: ({ row }: { row: Row<Screening> }) => {
      const { date } = formatDateTime(row.original.startDateTime);

      return <div>{date}</div>;
    },
  },
  {
    id: 'startTime',
    accessorKey: 'startTime',
    header: 'Godzina seansu',
    minSize: 50,
    cell: ({ row }: { row: Row<Screening> }) => {
      const { time: startTime } = formatDateTime(row.original.startDateTime);
      const { time: endTime } = formatDateTime(row.original.endDateTime);

      return <div>{`${startTime} - ${endTime}`}</div>;
    },
  },
];
