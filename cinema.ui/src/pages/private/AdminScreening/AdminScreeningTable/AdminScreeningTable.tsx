import { CirclePlus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

import { useAdminMovies } from 'Api/queries/useAdminMovies';
import { useAdminScreenings } from 'Api/queries/useAdminScreenings';
import { Button, ButtonSize } from 'Components/Button';
import { DataTable } from 'Components/DataTable';
import { Spinner } from 'Components/Spinner';
import { INITIAL_PARAMS } from 'Constants/index';
import { useTableState } from 'Hooks/useTableState';
import { ROUTES } from 'Routing/routes';
import { Screening } from 'Types/screening';

import { columns } from './columns';

export const AdminScreeningTable = () => {
  const { inputPhrase, table, isLoadingTableData, onSearchPhrase } = useTableState<Screening>(
    columns,
    INITIAL_PARAMS,
    useAdminScreenings
  );

  const { isFetching } = useAdminMovies({ page: 0, size: 0 });

  const navigate = useNavigate();

  const handleNavigate = () => navigate(ROUTES.private.SCREENING.ADD);

  return (
    <Spinner isSpinning={isLoadingTableData || isFetching}>
      <div className="space-y-4">
        <div className="flex justify-between">
          <span className="text-2xl">Seanse</span>
          <Button icon={<CirclePlus />} size={ButtonSize.Small} onClick={handleNavigate}>
            Dodaj
          </Button>
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
