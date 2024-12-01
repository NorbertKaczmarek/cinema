import { ComponentProps } from 'react';

import { ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight } from 'lucide-react';

import { Button, ButtonVariant } from 'Components/Button';
import { Select } from 'Components/Select';
import { cn } from 'Utils/cn';

interface Props {
  currentPageIndex?: number;
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

const DEFAULT_PAGINATION_VALUES = ['5', '10', '20'];

export const Pagination = ({
  navButtonClass,
  iconClass,
  currentPageIndex,
  maxPages,
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

  return (
    <div className={cn('flex items-center justify-end px-2', classNames?.mainWrapper)}>
      <div
        className={cn('flex items-center space-x-6 lg:space-x-8', classNames?.navigationWrapper)}
      >
        <div
          className={cn(
            'w-22 flex items-center justify-center text-sm font-medium',
            classNames?.pageOfWrapper
          )}
        >
          Strona {currentPageIndex} z {maxPages}
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
          <div className={cn('ml-2 flex items-center space-x-2', classNames?.changeSizeWrapper)}>
            <div className="min-w-32 text-sm font-medium">Wierszy na stronę</div>
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
