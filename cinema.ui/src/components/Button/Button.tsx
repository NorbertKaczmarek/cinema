import { ButtonHTMLAttributes, cloneElement, forwardRef, ReactElement } from 'react';
import { Slot } from '@radix-ui/react-slot';

import { type VariantProps } from 'class-variance-authority';
import { Loader2 } from 'lucide-react';

import { cn } from 'Utils/cn';

import { baseButtonVariants, ButtonSize, ButtonVariant } from './const';

interface BaseProps
  extends ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof baseButtonVariants> {
  asChild?: boolean;
}

const Base = forwardRef<HTMLButtonElement, BaseProps>(
  ({ className, variant, size, asChild = false, ...props }, ref) => {
    const Component = asChild ? Slot : 'button';

    return (
      <Component
        className={cn(baseButtonVariants({ variant, size, className }))}
        ref={ref}
        {...props}
      />
    );
  }
);
Base.displayName = 'BaseButton';

interface Props extends BaseProps {
  variant?: ButtonVariant;
  size?: ButtonSize;
  icon?: ReactElement;
  isButtonIcon?: boolean;
  isDisabled?: boolean;
  isLoading?: boolean;
}

export const Button = forwardRef<HTMLButtonElement, Props>(
  (
    {
      variant = ButtonVariant.Primary,
      size = ButtonSize.Default,
      type = 'button',
      className,
      icon,
      isButtonIcon,
      isDisabled,
      isLoading,
      onClick,
      children,
      ...rest
    }: Props,
    ref
  ) => (
    <Base
      variant={variant}
      size={size}
      type={type}
      className={className}
      disabled={isDisabled || isLoading}
      onClick={onClick}
      ref={ref}
      {...rest}
    >
      {!isLoading ? (
        <>
          {!!icon &&
            cloneElement(icon, {
              className: cn('h-4 w-4', isButtonIcon ? '' : 'mr-2', icon.props.className),
            })}
          {!isButtonIcon && children}
        </>
      ) : (
        <Loader2 className="animate-spin" />
      )}
    </Base>
  )
);
