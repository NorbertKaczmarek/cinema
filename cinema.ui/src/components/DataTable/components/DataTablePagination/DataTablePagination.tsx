import { Table } from '@tanstack/react-table';

import { Pagination } from 'Components/Pagination';

type DataTablePaginationProps<T> = {
  selectedRows?: number;
  maxRows?: number;
  table: Table<T>;
};

export const DataTablePagination = <T,>({
  selectedRows,
  maxRows,
  table,
}: DataTablePaginationProps<T>) => {
  const {
    getPageCount,
    getState,
    setPageSize,
    getCanPreviousPage,
    setPageIndex,
    previousPage,
    getCanNextPage,
    nextPage,
  } = table;

  return (
    <Pagination
      selectedRows={selectedRows}
      maxRows={maxRows}
      maxPages={getPageCount()}
      currentPageIndex={getState().pagination.pageIndex + 1}
      pageSizeValue={String(getState().pagination.pageSize)}
      onChangePageSize={value => setPageSize(Number(value))}
      isFirstPageDisabled={!getCanPreviousPage()}
      goToFirstPage={() => setPageIndex(0)}
      isPreviousPageDisabled={!getCanPreviousPage()}
      goToPreviousPage={() => previousPage()}
      isNextPageDisabled={!getCanNextPage()}
      goToNextPage={() => nextPage()}
      isLastPageDisabled={!getCanNextPage()}
      goToLastPage={() => setPageIndex(table.getPageCount() - 1)}
    />
  );
};
