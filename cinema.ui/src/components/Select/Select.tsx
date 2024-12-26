import { ComponentPropsWithoutRef, ElementRef, forwardRef } from 'react';
import * as SelectPrimitive from '@radix-ui/react-select';
import { SelectProps } from '@radix-ui/react-select';

import { Check, ChevronDown, ChevronUp } from 'lucide-react';

import { Label } from 'Components/Label';
import { cn } from 'Utils/cn';

const BaseWrapper = SelectPrimitive.Root;

const BaseValue = SelectPrimitive.Value;

const BaseTrigger = forwardRef<
  ElementRef<typeof SelectPrimitive.Trigger>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.Trigger>
>(({ className, children, ...props }, ref) => (
  <SelectPrimitive.Trigger
    ref={ref}
    className={cn(
      'my-[-2px] flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 [&>span]:line-clamp-1',
      className
    )}
    {...props}
  >
    {children}
    <SelectPrimitive.Icon asChild>
      <ChevronDown className="h-4 w-4 opacity-50" />
    </SelectPrimitive.Icon>
  </SelectPrimitive.Trigger>
));
BaseTrigger.displayName = SelectPrimitive.Trigger.displayName;

const BaseScrollUpButton = forwardRef<
  ElementRef<typeof SelectPrimitive.ScrollUpButton>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.ScrollUpButton>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.ScrollUpButton
    ref={ref}
    className={cn('flex cursor-default items-center justify-center py-1', className)}
    {...props}
  >
    <ChevronUp className="h-4 w-4" />
  </SelectPrimitive.ScrollUpButton>
));
BaseScrollUpButton.displayName = SelectPrimitive.ScrollUpButton.displayName;

const BaseScrollDownButton = forwardRef<
  ElementRef<typeof SelectPrimitive.ScrollDownButton>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.ScrollDownButton>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.ScrollDownButton
    ref={ref}
    className={cn('flex cursor-default items-center justify-center py-1', className)}
    {...props}
  >
    <ChevronDown className="h-4 w-4" />
  </SelectPrimitive.ScrollDownButton>
));
BaseScrollDownButton.displayName = SelectPrimitive.ScrollDownButton.displayName;

const BaseContent = forwardRef<
  ElementRef<typeof SelectPrimitive.Content>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.Content>
>(({ className, children, position = 'popper', ...props }, ref) => (
  <SelectPrimitive.Portal>
    <SelectPrimitive.Content
      ref={ref}
      className={cn(
        'relative z-50 max-h-64 min-w-[8rem] overflow-hidden rounded-md border bg-popover text-popover-foreground shadow-md data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[side=bottom]:slide-in-from-top-2 data-[side=left]:slide-in-from-right-2 data-[side=right]:slide-in-from-left-2 data-[side=top]:slide-in-from-bottom-2',
        position === 'popper' &&
          'data-[side=bottom]:translate-y-1 data-[side=left]:-translate-x-1 data-[side=right]:translate-x-1 data-[side=top]:-translate-y-1',
        className
      )}
      position={position}
      {...props}
    >
      <BaseScrollUpButton />
      <SelectPrimitive.Viewport
        className={cn(
          'p-1',
          position === 'popper' &&
            'h-[var(--radix-select-trigger-height)] w-full min-w-[var(--radix-select-trigger-width)]'
        )}
      >
        {children}
      </SelectPrimitive.Viewport>
      <BaseScrollDownButton />
    </SelectPrimitive.Content>
  </SelectPrimitive.Portal>
));
BaseContent.displayName = SelectPrimitive.Content.displayName;

const BaseLabel = forwardRef<
  ElementRef<typeof SelectPrimitive.Label>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.Label>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.Label
    ref={ref}
    className={cn(
      'text-sm font-semibold [&:not(:has(div))]:py-1.5 [&:not(:has(div))]:pl-8 [&:not(:has(div))]:pr-2',
      className
    )}
    {...props}
  />
));
BaseLabel.displayName = SelectPrimitive.Label.displayName;

const BaseItem = forwardRef<
  ElementRef<typeof SelectPrimitive.Item>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.Item>
>(({ className, children, ...props }, ref) => (
  <SelectPrimitive.Item
    ref={ref}
    className={cn(
      'relative flex w-full cursor-default select-none items-center rounded-sm py-1.5 pl-8 pr-2 text-sm outline-none focus:bg-accent focus:text-accent-foreground data-[disabled]:pointer-events-none data-[disabled]:opacity-50',
      className
    )}
    {...props}
  >
    <span className="absolute left-2 flex h-3.5 w-3.5 items-center justify-center">
      <SelectPrimitive.ItemIndicator>
        <Check className="h-4 w-4" />
      </SelectPrimitive.ItemIndicator>
    </span>

    <SelectPrimitive.ItemText>{children}</SelectPrimitive.ItemText>
  </SelectPrimitive.Item>
));
BaseItem.displayName = SelectPrimitive.Item.displayName;

const BaseSeparator = forwardRef<
  ElementRef<typeof SelectPrimitive.Separator>,
  ComponentPropsWithoutRef<typeof SelectPrimitive.Separator>
>(({ className, ...props }, ref) => (
  <SelectPrimitive.Separator
    ref={ref}
    className={cn('-mx-1 my-1 h-px bg-muted', className)}
    {...props}
  />
));
BaseSeparator.displayName = SelectPrimitive.Separator.displayName;

interface SelectOption {
  value: string | number;
  label?: string;
  isDisabled?: boolean;
}

interface Props extends SelectProps {
  options?: SelectOption[];
  label?: string;
  value?: string;
  classNames?: ClassNames<
    'content' | 'trigger' | 'input' | 'inputValue' | 'inputPlaceholder' | 'item' | 'group'
  >;
  placeholder?: string;
  onChange?: (value: string) => void;
}

export const Select = forwardRef<HTMLDivElement, Props>(
  (
    {
      label,
      options,
      placeholder = 'Wybierz',
      value,
      onChange,
      classNames,
      children,
      ...rest
    }: Props,
    ref
  ) => (
    <BaseWrapper onValueChange={onChange} value={value} {...rest}>
      {!!label && <Label>{label}</Label>}
      <BaseTrigger className={cn('relative', classNames?.input)}>
        <BaseValue
          ref={ref}
          className={classNames?.inputValue}
          placeholder={
            <span className={cn('opacity-60', classNames?.inputPlaceholder)}>{placeholder}</span>
          }
        />
      </BaseTrigger>
      <BaseContent className={classNames?.content} ref={ref}>
        {options?.length
          ? options.map(({ label, value, isDisabled }, index) => (
              <BaseItem key={`${index}-${label}`} value={value.toString()} disabled={isDisabled}>
                {label || value}
              </BaseItem>
            ))
          : children}
      </BaseContent>
    </BaseWrapper>
  )
);

export const BaseSelect = {
  Wrapper: BaseWrapper,
  Trigger: BaseTrigger,
  Value: BaseValue,
  Content: BaseContent,
  Item: BaseItem,
};
