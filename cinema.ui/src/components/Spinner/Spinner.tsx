import { cloneElement, ReactElement, ReactNode } from 'react';

import { Loader2 } from 'lucide-react';

import { cn } from 'Utils/cn';

interface Props {
  text?: string;
  icon?: ReactElement;
  customSpinner?: ReactNode;
  classNames?: ClassNames<'wrapper' | 'spinnerContainer' | 'spinner' | 'text' | 'icon'>;
  isSpinning?: boolean;
  children?: ReactNode;
}

export const Spinner = ({
  text,
  icon,
  customSpinner,
  classNames,
  isSpinning = false,
  children,
}: Props) => {
  if (!isSpinning) return children;

  return (
    <div className={cn('relative h-full w-full', classNames?.wrapper)}>
      <div
        className={cn(
          'absolute left-0 top-0 z-50 flex h-full w-full items-center justify-center bg-white bg-opacity-70',
          classNames?.spinnerContainer
        )}
      >
        {customSpinner ?? (
          <div className={cn('flex items-center gap-2 text-foreground', classNames?.spinner)}>
            {!!text && <p className={cn('text-sm', classNames?.text)}>{text}</p>}
            {cloneElement(icon || <Loader2 />, {
              className: cn('animate-spin', classNames?.icon),
            })}
          </div>
        )}
      </div>
      {children}
    </div>
  );
};
