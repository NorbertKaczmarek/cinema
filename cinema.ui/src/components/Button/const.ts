import { cva } from 'class-variance-authority';

export const baseButtonVariants = cva(
  'inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50',
  {
    variants: {
      variant: {
        default: 'bg-primary text-primary-foreground shadow hover:bg-primary/90',
        outline:
          'border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground',
        secondary: 'bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80',
        ghost: 'hover:bg-accent hover:text-accent-foreground',
        link: 'text-primary underline-offset-4 hover:underline',
        destructive: 'bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/80',
        success: 'bg-success text-success-foreground shadow-sm hover:bg-success/80',
        warning: 'bg-warning text-warning-foreground shadow-sm hover:bg-warning/80',
      },
      size: {
        default: 'h-9 px-4 py-2',
        sm: 'h-8 rounded-md px-3 text-xs',
        lg: 'h-10 rounded-md px-8',
        icon: 'h-7 w-7',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'default',
    },
  }
);

export enum ButtonVariant {
  Primary = 'default',
  Secondary = 'secondary',
  Outline = 'outline',
  Ghost = 'ghost',
  Link = 'link',
  Success = 'success',
  Warning = 'warning',
  Danger = 'destructive',
}

export enum ButtonSize {
  Default = 'default',
  Small = 'sm',
  Large = 'lg',
  Icon = 'icon',
}
