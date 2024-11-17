import { Table } from '@tanstack/react-table';

import { Pagination } from 'Components/Pagination';

export const DataTablePagination = <T,>({ table }: { table: Table<T> }) => {
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
