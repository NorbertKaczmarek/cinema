import { ComponentPropsWithoutRef, ElementRef, forwardRef } from 'react';
import { Root, SeparatorProps as BaseProps } from '@radix-ui/react-separator';

import { cn } from 'Utils/cn';

const Base = forwardRef<ElementRef<typeof Root>, ComponentPropsWithoutRef<typeof Root>>(
  ({ className, orientation = 'horizontal', decorative = true, ...props }, ref) => (
    <Root
      ref={ref}
      decorative={decorative}
      orientation={orientation}
      className={cn(
        'shrink-0 bg-border',
        orientation === 'horizontal' ? 'h-[1px] w-full' : 'h-full w-[1px]',
        className
      )}
      {...props}
    />
  )
);
Base.displayName = Root.displayName;

interface Props extends BaseProps {
  isVertical?: boolean;
}

export const Separator = forwardRef<HTMLDivElement, Props>(({ isVertical, ...rest }, ref) => (
  <Base ref={ref} orientation={isVertical ? 'vertical' : 'horizontal'} {...rest} />
));
