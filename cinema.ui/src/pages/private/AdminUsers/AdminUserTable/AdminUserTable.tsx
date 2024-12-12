import { CirclePlus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

import { useAdminUsers } from 'Api/queries/useAdminUsers';
import { Button, ButtonSize } from 'Components/Button';
import { DataTable } from 'Components/DataTable';
import { Spinner } from 'Components/Spinner';
import { INITIAL_PARAMS } from 'Constants/index';
import { useTableState } from 'Hooks/useTableState';
import { ROUTES } from 'Routing/routes';
import { User } from 'Types/user';

import { columns } from './columns';

export const AdminUserTable = () => {
  const { inputPhrase, table, isLoadingTableData, onSearchPhrase } = useTableState<User>(
    columns,
    INITIAL_PARAMS,
    useAdminUsers
  );

  const navigate = useNavigate();

  const handleNavigate = () => navigate(ROUTES.private.USER.ADD);

  return (
    <Spinner isSpinning={isLoadingTableData}>
      <div className="space-y-4">
        <div className="flex justify-between">
          <span className="text-2xl">Pracownicy</span>
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
