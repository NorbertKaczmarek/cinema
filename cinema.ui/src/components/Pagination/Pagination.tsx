import { ComponentProps } from 'react';

import { ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight } from 'lucide-react';

import { Button, ButtonVariant } from 'Components/Button';
import { Select } from 'Components/Select';
import { cn } from 'Utils/cn';

interface Props {
  currentPageIndex?: number;
  selectedRows?: number;
  maxRows?: number;
  maxPages?: number;
  pageSizeValue?: string;
  navButtonClass?: string;
  iconClass?: string;
  pageSizeOptions?: ComponentProps<typeof Select>['options'];
  onChangePageSize?: (value: string) => void;
  isFirstPageDisabled?: boolean;
  goToFirstPage?: () => void;
  isPreviousPageDisabled?: boolean;
  goToPreviousPage?: () => void;
  isNextPageDisabled?: boolean;
  goToNextPage?: () => void;
  isLastPageDisabled?: boolean;
  goToLastPage?: () => void;
  classNames?: ClassNames<
    | 'mainWrapper'
    | 'selectedRowsWrapper'
    | 'navigationWrapper'
    | 'changeSizeWrapper'
    | 'buttonWrapper'
    | 'pageOfWrapper'
  >;
}

const DEFAULT_PAGINATION_VALUES = ['5', '10', '20', '50', '100'];

export const Pagination = ({
  navButtonClass,
  iconClass,
  currentPageIndex,
  maxRows,
  maxPages,
  selectedRows,
  pageSizeValue,
  onChangePageSize,
  pageSizeOptions,
  isFirstPageDisabled,
  goToFirstPage,
  isPreviousPageDisabled,
  goToPreviousPage,
  isNextPageDisabled,
  goToNextPage,
  isLastPageDisabled,
  goToLastPage,
  classNames,
}: Props) => {
  const defaultSelectOptions = DEFAULT_PAGINATION_VALUES.map(el => ({ label: el, value: el }));
  const isSelectedRows = !!selectedRows && !!maxRows;

  return (
    <div
      className={cn(
        `flex items-center ${isSelectedRows ? 'justify-between' : 'justify-end'} px-2`,
        classNames?.mainWrapper
      )}
    >
      {isSelectedRows && (
        <div
          className={cn('flex-1 text-sm text-muted-foreground', classNames?.selectedRowsWrapper)}
        >
          {selectedRows} of {maxRows} row(s) selected.
        </div>
      )}
      <div
        className={cn('flex items-center space-x-6 lg:space-x-8', classNames?.navigationWrapper)}
      >
        <div
          className={cn(
            'flex w-24 items-center justify-center text-sm font-medium',
            classNames?.pageOfWrapper
          )}
        >
          Page {currentPageIndex} of {maxPages}
        </div>
        <div className={cn('flex items-center space-x-2', classNames?.buttonWrapper)}>
          {goToFirstPage && (
            <Button
              variant={ButtonVariant.Outline}
              className={cn('hidden h-8 w-8 p-0 lg:flex', navButtonClass)}
              onClick={goToFirstPage}
              disabled={isFirstPageDisabled}
            >
              <ChevronsLeft className={cn('h-4 w-4', iconClass)} />
            </Button>
          )}
          {goToPreviousPage && (
            <Button
              variant={ButtonVariant.Outline}
              className={cn('h-8 w-8 p-0', navButtonClass)}
              onClick={goToPreviousPage}
              disabled={isPreviousPageDisabled}
            >
              <ChevronLeft className={cn('h-4 w-4', iconClass)} />
            </Button>
          )}
          {goToNextPage && (
            <Button
              variant={ButtonVariant.Outline}
              className={cn('h-8 w-8 p-0', navButtonClass)}
              onClick={goToNextPage}
              disabled={isNextPageDisabled}
            >
              <ChevronRight className={cn('h-4 w-4', iconClass)} />
            </Button>
          )}
          {goToLastPage && (
            <Button
              variant={ButtonVariant.Outline}
              className={cn('hidden h-8 w-8 p-0 lg:flex', navButtonClass)}
              onClick={goToLastPage}
              disabled={isLastPageDisabled}
            >
              <ChevronsRight className={cn('h-4 w-4', iconClass)} />
            </Button>
          )}
        </div>
        {onChangePageSize && (
          <div className={cn('ml-4 flex items-center space-x-2', classNames?.changeSizeWrapper)}>
            <div className="min-w-24 text-sm font-medium">Rows per page</div>
            <Select
              value={pageSizeValue}
              onChange={onChangePageSize}
              options={pageSizeOptions ?? defaultSelectOptions}
            />
          </div>
        )}
      </div>
    </div>
  );
};
