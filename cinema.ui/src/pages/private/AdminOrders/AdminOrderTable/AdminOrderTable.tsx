import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { useAdminOrders } from 'Api/queries/useAdminOrders';
import { useAdminScreenings } from 'Api/queries/useAdminScreenings';
import { DataTable } from 'Components/DataTable';
import { Spinner } from 'Components/Spinner';
import { INITIAL_PARAMS } from 'Constants/index';
import { useTableState } from 'Hooks/useTableState';
import { Order } from 'Types/order';

import { columns } from './columns';

export const AdminOrderTable = () => {
  const { inputPhrase, table, isLoadingTableData, onSearchPhrase } = useTableState<Order>(
    columns,
    INITIAL_PARAMS,
    useAdminOrders
  );

  const { isFetching: isLoadingScreenings } = useAdminScreenings({ page: 0, size: 0 });
  const { isFetching: isLoadingMovies } = useAdminMovies({ page: 0, size: 0 });

  return (
    <Spinner isSpinning={isLoadingTableData || isLoadingScreenings || isLoadingMovies}>
      <div className="space-y-4">
        <div className="flex justify-between">
          <span className="text-2xl">Zamówienia</span>
        </div>
        <div className="flex w-full flex-col justify-between lg:flex-row">
          <div className="flex w-full flex-col space-y-2 lg:w-auto lg:flex-row lg:flex-wrap lg:space-x-1 lg:space-y-0">
            <DataTable.Search
              placeholder="Wyszukaj"
              value={inputPhrase}
              onChange={onSearchPhrase}
            />
          </div>
        </div>
        <DataTable table={table} columns={columns} notFoundText="Brak wyników" />
        <DataTable.Pagination table={table} />
      </div>
    </Spinner>
  );
};
