import { ComponentPropsWithoutRef, ElementRef, forwardRef, ReactNode } from 'react';
import * as TooltipPrimitive from '@radix-ui/react-tooltip';
import { TooltipContentProps } from '@radix-ui/react-tooltip';

import { InfoIcon } from 'lucide-react';

import { cn } from 'Utils/cn';

const BaseProvider = TooltipPrimitive.Provider;

const BaseWrapper = TooltipPrimitive.Root;

const BaseTrigger = TooltipPrimitive.Trigger;

const BaseContent = forwardRef<
  ElementRef<typeof TooltipPrimitive.Content>,
  ComponentPropsWithoutRef<typeof TooltipPrimitive.Content>
>(({ className, sideOffset = 4, ...props }, ref) => (
  <TooltipPrimitive.Content
    ref={ref}
    sideOffset={sideOffset}
    className={cn(
      'z-50 overflow-hidden rounded-md border bg-popover px-3 py-1.5 text-sm text-popover-foreground shadow-md animate-in fade-in-0 zoom-in-95 data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=closed]:zoom-out-95 data-[side=bottom]:slide-in-from-top-2 data-[side=left]:slide-in-from-right-2 data-[side=right]:slide-in-from-left-2 data-[side=top]:slide-in-from-bottom-2',
      className
    )}
    {...props}
  />
));
BaseContent.displayName = TooltipPrimitive.Content.displayName;

interface Props extends TooltipContentProps {
  text?: string;
  classNames?: ClassNames<'trigger' | 'icon' | 'content' | 'text'>;
  trigger?: ReactNode;
  children?: ReactNode;
}

export const Tooltip = forwardRef<HTMLDivElement, Props>(
  ({ text, classNames, trigger, children, asChild = true, ...rest }: Props, ref) => (
    <BaseProvider>
      <BaseWrapper>
        <BaseTrigger className={classNames?.trigger} asChild={asChild}>
          {trigger || <InfoIcon className={cn('h-4 w-4 text-foreground', classNames)} />}
        </BaseTrigger>
        {!!(children || text) && (
          <BaseContent ref={ref} className={classNames?.content} {...rest}>
            {children} {!!text && <p className={classNames?.text}>{text}</p>}
          </BaseContent>
        )}
      </BaseWrapper>
    </BaseProvider>
  )
);
