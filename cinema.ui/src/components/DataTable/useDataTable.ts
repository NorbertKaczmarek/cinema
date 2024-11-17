import { ChangeEvent, useState } from 'react';
import { PaginationState, RowSelectionState } from '@tanstack/react-table';
import { OnChangeFn } from '@tanstack/table-core/src/types';

import { DEFAULT_PAGE_INDEX, MIN_SEARCH_PHRASE } from 'Constants/index';
import useDebouncePhrase from 'Hooks/useDebouncedPhrase';
import { ParamsState } from 'Types/table';

export const useDataTable = (initialParams: ParamsState) => {
  const [rowSelection, setRowSelection] = useState<RowSelectionState>({});
  const [inputPhrase, setInputPhrase] = useState('');
  const [params, setParams] = useState<ParamsState>(initialParams || {});

  const { onSearch } = useDebouncePhrase(MIN_SEARCH_PHRASE, (value?: string) =>
    setParams(prevState => ({
      ...prevState,
      phrase: value,
      pagination: {
        ...prevState.pagination,
        pageIndex: DEFAULT_PAGE_INDEX,
      },
    }))
  );

  const onSearchPhrase = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setInputPhrase(value);
    onSearch(value);
  };

  const onPaginationChange: OnChangeFn<PaginationState> = paginationUpdater => {
    setParams(prevParams => ({
      ...prevParams,
      pagination:
        typeof paginationUpdater === 'function'
          ? paginationUpdater(prevParams.pagination)
          : paginationUpdater,
    }));
  };

  return {
    inputPhrase,
    params,
    rowSelection,
    onPaginationChange,
    onSearchPhrase,
    setRowSelection,
  };
};
