import { ComponentPropsWithoutRef, ElementRef, forwardRef, ReactNode } from 'react';
import {
  Corner,
  Root,
  ScrollAreaProps as BaseScrollAreaProps,
  ScrollAreaScrollbar,
  ScrollAreaThumb,
  Viewport,
} from '@radix-ui/react-scroll-area';

import { cn } from 'Utils/cn';

const BaseScrollBar = forwardRef<
  ElementRef<typeof ScrollAreaScrollbar>,
  ComponentPropsWithoutRef<typeof ScrollAreaScrollbar>
>(({ className, orientation = 'vertical', ...props }, ref) => (
  <ScrollAreaScrollbar
    ref={ref}
    orientation={orientation}
    className={cn(
      'flex touch-none select-none transition-colors',
      orientation === 'vertical' && 'h-full w-2.5 border-l border-l-transparent p-[1px]',
      orientation === 'horizontal' && 'h-2.5 flex-col border-t border-t-transparent p-[1px]',
      className
    )}
    {...props}
  >
    <ScrollAreaThumb className="relative flex-1 rounded-full bg-border" />
  </ScrollAreaScrollbar>
));
BaseScrollBar.displayName = ScrollAreaScrollbar.displayName;

export const BaseScrollArea = forwardRef<
  ElementRef<typeof Root>,
  ComponentPropsWithoutRef<typeof Root>
>(({ className, children, ...props }, ref) => (
  <Root ref={ref} className={cn('relative overflow-hidden', className)} {...props}>
    <Viewport className="h-full w-full rounded-[inherit]">{children}</Viewport>
    <BaseScrollBar />
    <Corner />
  </Root>
));
BaseScrollArea.displayName = Root.displayName;

interface Props extends BaseScrollAreaProps {
  className?: string;
  children?: ReactNode;
  orientation?: 'vertical' | 'horizontal';
}

export const ScrollArea = forwardRef<HTMLDivElement, Props>(
  ({ className, children, orientation }: Props, ref) => (
    <BaseScrollArea
      className={cn(orientation === 'horizontal' && 'whitespace-nowrap', className)}
      aria-orientation={orientation}
      ref={ref}
    >
      {children}
    </BaseScrollArea>
  )
);
