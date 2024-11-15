import { cva } from 'class-variance-authority';

export const badgeVariants = cva(
  'inline-flex min-w-20 max-w-max items-center justify-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2',
  {
    variants: {
      variant: {
        default: 'border-transparent bg-primary text-primary-foreground hover:bg-primary/80',
        disabled: 'border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80',
        danger:
          'border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80',
        success: 'border-transparent bg-success text-success-foreground hover:bg-success/80',
        warning: 'border-transparent bg-warning text-warning-foreground hover:bg-warning/80',
        info: 'border-transparent bg-info text-info-foreground hover:bg-info/80',
        outline: 'text-foreground',
      },
    },
    defaultVariants: {
      variant: 'default',
    },
  }
);

export enum BadgeVariant {
  Default = 'default',
  Disabled = 'disabled',
  Danger = 'danger',
  Success = 'success',
  Info = 'info',
  Warning = 'warning',
  Outline = 'outline',
}
