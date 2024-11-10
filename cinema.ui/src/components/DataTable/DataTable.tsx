import { ReactNode } from 'react';
import { ColumnDef, flexRender, Table as TanstackTable } from '@tanstack/react-table';

import { DataTablePagination, DataTableSearch } from 'Components/DataTable/components';
import { Table, TableProps } from 'Components/Table';
import { cn } from 'Utils/cn';

export type DataTableProps<T, P> = {
  table: TanstackTable<T>;
  columns: ColumnDef<T, P>[];
  classNames?: ClassNames<'mainWrapper'> & TableProps['classNames'];
  notFoundText: ReactNode;
} & Omit<TableProps, 'columns' | 'content'>;

export const DataTable = <T, P>({
  table,
  columns,
  notFoundText,
  classNames,
  ...rest
}: DataTableProps<T, P>) => {
  const emptyTable = (
    <Table.Row>
      <Table.Cell colSpan={columns.length} className="h-24 text-center">
        {notFoundText}
      </Table.Cell>
    </Table.Row>
  );

  return (
    <div
      className={cn(
        'block overflow-x-auto whitespace-nowrap rounded-md border',
        classNames?.mainWrapper
      )}
    >
      <Table
        classNames={{
          wrapper: 'table-fixed bg-white',
          ...classNames,
        }}
        customColumns={
          <Table.Header className={classNames?.header}>
            {table.getHeaderGroups().map(({ id, headers }) => (
              <Table.Row key={id} className={classNames?.row}>
                {headers.map(header => (
                  <Table.Head
                    key={header.id}
                    style={{ width: header.getSize() }}
                    className={cn('h-12', classNames?.head)}
                  >
                    {header.isPlaceholder
                      ? null
                      : flexRender(header.column.columnDef.header, header.getContext())}
                  </Table.Head>
                ))}
              </Table.Row>
            ))}
          </Table.Header>
        }
        customContent={
          <>
            {table.getRowModel().rows?.length
              ? table.getRowModel().rows.map(row => (
                  <Table.Row
                    key={row.id}
                    data-state={row.getIsSelected() && 'selected'}
                    className={cn('h-12', classNames?.row)}
                  >
                    {row.getVisibleCells().map(cell => (
                      <Table.Cell
                        key={cell.id}
                        style={{ width: cell.column.getSize() }}
                        className={classNames?.cell}
                      >
                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                      </Table.Cell>
                    ))}
                  </Table.Row>
                ))
              : emptyTable}
          </>
        }
        {...rest}
      />
    </div>
  );
};

DataTable.Search = DataTableSearch;
DataTable.Pagination = DataTablePagination;

export default DataTable;
