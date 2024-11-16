import { forwardRef, ReactNode, TextareaHTMLAttributes } from 'react';

import { Label } from 'Components/Label';
import { cn } from 'Utils/cn';

type BaseTextareaProps = TextareaHTMLAttributes<HTMLTextAreaElement>;

const BaseTextarea = forwardRef<HTMLTextAreaElement, BaseTextareaProps>(
  ({ className, ...props }, ref) => {
    return (
      <textarea
        className={cn(
          'flex min-h-[100px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50',
          className
        )}
        ref={ref}
        {...props}
      />
    );
  }
);
BaseTextarea.displayName = 'Textarea';

interface Props extends BaseTextareaProps {
  placeholder?: string;
  classNames?: ClassNames<'wrapper' | 'textarea'>;
  htmlFor?: string;
  label?: ReactNode;
  footer?: ReactNode;
  header?: ReactNode;
  isDisabled?: boolean;
}

export const Textarea = forwardRef<HTMLTextAreaElement, Props>(
  (
    { isDisabled, placeholder, classNames, footer, header, label, htmlFor, ...rest }: Props,
    ref
  ) => (
    <div className={cn('grid w-full gap-1.5', classNames?.wrapper)}>
      {header}
      {label && <Label htmlFor={htmlFor}>{label}</Label>}
      <BaseTextarea
        ref={ref}
        disabled={isDisabled}
        placeholder={placeholder}
        className={classNames?.textarea}
        {...rest}
      />
      {footer}
    </div>
  )
);
