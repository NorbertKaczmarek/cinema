import { ComponentPropsWithoutRef, ElementRef, forwardRef, ReactNode } from 'react';
import { CheckboxProps, Indicator, Root } from '@radix-ui/react-checkbox';

import { Check } from 'lucide-react';
import { ControllerRenderProps } from 'react-hook-form';

import { Label } from 'Components/Label';
import { cn } from 'Utils/cn.ts';

const Base = forwardRef<ElementRef<typeof Root>, ComponentPropsWithoutRef<typeof Root>>(
  ({ className, ...props }, ref) => (
    <Root
      ref={ref}
      className={cn(
        'peer h-4 w-4 shrink-0 rounded-sm border border-primary ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 data-[state=checked]:bg-primary data-[state=checked]:text-primary-foreground',
        className
      )}
      {...props}
    >
      <Indicator className={cn('flex items-center justify-center text-current')}>
        <Check className="h-4 w-4" />
      </Indicator>
    </Root>
  )
);

Base.displayName = Root.displayName;

type Props = CheckboxProps &
  Partial<ControllerRenderProps> & {
    name: string;
    classNames?: ClassNames<'wrapper' | 'checkbox' | 'label'>;
    isDisabled?: boolean;
    isChecked?: boolean;
    isDefaultChecked?: boolean;
    onCheckedChange?: (value: boolean) => void;
  } & ({ label: string; customLabel?: undefined } | { label?: undefined; customLabel: ReactNode });

export const Checkbox = forwardRef<HTMLButtonElement, Props>(
  (
    {
      name,
      label,
      customLabel,
      classNames,
      value,
      isDisabled = false,
      isChecked,
      isDefaultChecked = false,
      onChange,
      onCheckedChange,
      ...rest
    }: Props,
    ref
  ) => (
    <div className={cn('flex space-x-2', classNames?.wrapper)}>
      <Base
        ref={ref}
        name={name}
        className={classNames?.checkbox}
        disabled={isDisabled}
        checked={isChecked || value}
        defaultChecked={isDefaultChecked}
        onCheckedChange={onCheckedChange || onChange}
        {...rest}
      />
      {!!label && (
        <Label htmlFor={name} className={cn(isDisabled && 'text-muted', classNames?.label)}>
          {label}
        </Label>
      )}
      {!!customLabel && customLabel}
    </div>
  )
);
