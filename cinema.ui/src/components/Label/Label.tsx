import { ComponentPropsWithoutRef, ElementRef, forwardRef, ReactNode } from 'react';
import { LabelProps, Root } from '@radix-ui/react-label';

import { VariantProps } from 'class-variance-authority';

import { cn } from 'Utils/cn.ts';

import { labelVariants } from './const.ts';

const Base = forwardRef<
  ElementRef<typeof Root>,
  ComponentPropsWithoutRef<typeof Root> & VariantProps<typeof labelVariants>
>(({ className, ...props }, ref) => (
  <Root ref={ref} className={cn(labelVariants(), className)} {...props} />
));
Base.displayName = Root.displayName;

interface Props extends LabelProps {
  htmlFor?: string;
  className?: string;
  children?: ReactNode;
}

export const Label = forwardRef<HTMLLabelElement, Props>(
  ({ htmlFor, className, children }: Props, ref) => (
    <Base htmlFor={htmlFor} className={className} ref={ref}>
      {children}
    </Base>
  )
);
