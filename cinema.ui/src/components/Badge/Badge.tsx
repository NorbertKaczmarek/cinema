import { HTMLAttributes, ReactNode } from 'react';

import { type VariantProps } from 'class-variance-authority';

import { cn } from 'Utils/cn';

import { BadgeVariant, badgeVariants } from './const.ts';

interface Props extends HTMLAttributes<HTMLDivElement>, VariantProps<typeof badgeVariants> {
  variant?: BadgeVariant;
  className?: string;
  children?: ReactNode;
}

export const Badge = ({ variant = BadgeVariant.Default, className, children, ...rest }: Props) => (
  <div className={cn(badgeVariants({ variant }), className)} {...rest}>
    {children}
  </div>
);
