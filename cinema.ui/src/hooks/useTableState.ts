import { UseQueryResult } from '@tanstack/react-query';
import { ColumnDef, getCoreRowModel, useReactTable } from '@tanstack/react-table';

import { useDataTable } from 'Components/DataTable/useDataTable';
import { BackendTable, ParamsState, QueryParamsTable } from 'Types/table';

export const useTableState = <T>(
  columns: ColumnDef<T>[],
  initialParams: ParamsState,
  fetchTableQuery: (params: QueryParamsTable) => UseQueryResult<BackendTable<T>, Error>
) => {
  const {
    inputPhrase,
    params: { pagination, phrase },
    rowSelection,
    setRowSelection,
    onSearchPhrase,
    onPaginationChange,
  } = useDataTable(initialParams);

  const { data, isFetching: isLoadingTableData } = fetchTableQuery({
    page: pagination.pageIndex,
    size: pagination.pageSize,
    phrase,
  });

  const table = useReactTable({
    data: data?.content || [],
    columns,
    pageCount: data?.totalPages,
    getCoreRowModel: getCoreRowModel(),
    manualFiltering: true,
    manualPagination: true,
    onPaginationChange: onPaginationChange,
    manualSorting: true,
    onRowSelectionChange: setRowSelection,
    state: {
      pagination,
      rowSelection,
    },
  });

  const selectedRows = Object.keys(rowSelection)?.length;

  return {
    inputPhrase,
    selectedRows,
    table,
    data,
    isLoadingTableData,
    onSearchPhrase,
  };
};
