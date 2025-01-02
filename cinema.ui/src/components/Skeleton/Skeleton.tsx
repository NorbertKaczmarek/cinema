import { FC, HTMLAttributes } from 'react';

import { cn } from 'Utils/cn';

const BaseSkeleton = ({ className, ...props }: HTMLAttributes<HTMLDivElement>) => {
  return <div className={cn('animate-pulse rounded-md bg-muted', className)} {...props} />;
};

const gridColumns: Record<number, string> = {
  1: 'grid-cols-1',
  2: 'grid-cols-2',
  3: 'grid-cols-3',
  4: 'grid-cols-4',
  5: 'grid-cols-5',
  6: 'grid-cols-6',
  7: 'grid-cols-7',
  8: 'grid-cols-8',
  9: 'grid-cols-9',
  10: 'grid-cols-10',
  11: 'grid-cols-11',
  12: 'grid-cols-12',
};

export interface SkeletonProps {
  rows?: number;
  columns?: number;
  classNames?: ClassNames<'wrapper' | 'skeleton'>;
}

export const Skeleton: FC<SkeletonProps> = ({ rows = 1, columns = 1, classNames }) => {
  const skeletonCount = rows * columns;

  return (
    <div className={cn('grid h-full w-full gap-4', gridColumns[columns], classNames?.wrapper)}>
      {Array.from({ length: skeletonCount }).map((_, index) => (
        <BaseSkeleton key={index} className={cn('h-full w-full', classNames?.skeleton)} />
      ))}
    </div>
  );
};
