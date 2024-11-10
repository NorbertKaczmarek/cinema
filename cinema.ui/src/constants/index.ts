import { ParamsState } from 'Types/table';

export const DEFAULT_PAGE_INDEX = 0;
export const DEFAULT_PAGE_SIZE = 5;
export const MIN_SEARCH_PHRASE = 3;
export const DEFAULT_DATE_FORMAT = 'dd.MM.yyyy';

export const INITIAL_PARAMS: ParamsState = {
  phrase: undefined,
  pagination: {
    pageIndex: DEFAULT_PAGE_INDEX,
    pageSize: DEFAULT_PAGE_SIZE,
  },
};
