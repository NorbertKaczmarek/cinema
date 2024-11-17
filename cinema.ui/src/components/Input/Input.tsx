import { forwardRef, InputHTMLAttributes, useState } from 'react';

import { EyeIcon, EyeOffIcon } from 'lucide-react';

import { Button, ButtonSize, ButtonVariant } from 'Components/Button';
import { Label } from 'Components/Label';
import { cn } from 'Utils/cn';

type BaseProps = InputHTMLAttributes<HTMLInputElement>;

const Base = forwardRef<HTMLInputElement, BaseProps>(({ className, type, ...props }, ref) => (
  <input
    type={type}
    className={cn(
      'flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background transition-all duration-100 ease-in-out file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50',
      className
    )}
    ref={ref}
    {...props}
  />
));
Base.displayName = 'Input';

type InputClassNames = 'wrapper' | 'input';

export interface Props extends BaseProps {
  label?: string;
  classNames?: ClassNames<InputClassNames>;
  isDisabled?: boolean;
}

export const Input = forwardRef<HTMLInputElement, Props>(
  ({ label, type, name, isDisabled, classNames, ...rest }: Props, ref) => {
    const isPassword = type === 'password';

    const [isPasswordVisible, setIsPasswordVisible] = useState(!isPassword);
    const inputType = isPassword && isPasswordVisible ? 'text' : type;

    const togglePasswordVisibility = () => setIsPasswordVisible(oldState => !oldState);

    return (
      <div className={cn('flex flex-col gap-1.5', classNames?.wrapper)}>
        {label && <Label htmlFor={name}>{label}</Label>}
        <div className="relative flex flex-row gap-1.5">
          <Base
            ref={ref}
            className={cn(isPassword && 'pr-12', classNames?.input)}
            type={inputType}
            name={name}
            disabled={isDisabled}
            {...rest}
          />
          {isPassword && (
            <Button
              className="absolute right-1 top-1 mr-0.5 mt-0.5"
              type="button"
              variant={ButtonVariant.Ghost}
              size={ButtonSize.Icon}
              icon={isPasswordVisible ? <EyeIcon /> : <EyeOffIcon />}
              isButtonIcon
              onClick={togglePasswordVisibility}
            />
          )}
        </div>
      </div>
    );
  }
);
