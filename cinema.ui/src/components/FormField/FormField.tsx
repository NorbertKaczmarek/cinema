import {
  cloneElement,
  ComponentPropsWithoutRef,
  createContext,
  ElementRef,
  forwardRef,
  HTMLAttributes,
  ReactElement,
  useContext,
  useId,
} from 'react';
import { Root as LabelRoot } from '@radix-ui/react-label';
import { Slot } from '@radix-ui/react-slot';

import {
  Controller,
  ControllerProps,
  FieldPath,
  FieldValues,
  FormProvider,
  UseControllerProps,
  useFormContext,
} from 'react-hook-form';

import { Label as FormLabel } from 'Components/Label';
import { cn } from 'Utils/cn';

const Form = FormProvider;

type FormFieldContextValue<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
};

const FormFieldContext = createContext<FormFieldContextValue>({} as FormFieldContextValue);

type FormItemContextValue = {
  id: string;
};

const FormItemContext = createContext<FormItemContextValue>({} as FormItemContextValue);

const BaseField = <
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
>({
  ...props
}: ControllerProps<TFieldValues, TName>) => (
  <FormFieldContext.Provider value={{ name: props.name }}>
    <Controller {...props} />
  </FormFieldContext.Provider>
);

const useFormField = () => {
  const fieldContext = useContext(FormFieldContext);
  const itemContext = useContext(FormItemContext);
  const { getFieldState, formState } = useFormContext();

  const fieldState = getFieldState(fieldContext.name, formState);

  if (!fieldContext) {
    throw new Error('useFormField should be used within <Form.Field>');
  }

  const { id } = itemContext;

  return {
    id,
    name: fieldContext.name,
    formItemId: `${id}-form-item`,
    formDescriptionId: `${id}-form-item-description`,
    formMessageId: `${id}-form-item-message`,
    ...fieldState,
  };
};

const BaseItem = forwardRef<HTMLDivElement, HTMLAttributes<HTMLDivElement>>(
  ({ className, ...props }, ref) => {
    const id = useId();

    return (
      <FormItemContext.Provider value={{ id }}>
        <div ref={ref} className={cn('flex flex-col gap-2', className)} {...props} />
      </FormItemContext.Provider>
    );
  }
);
BaseItem.displayName = 'FormItem';

const BaseLabel = forwardRef<
  ElementRef<typeof LabelRoot>,
  ComponentPropsWithoutRef<typeof LabelRoot>
>(({ className, ...props }, ref) => {
  const { error, formItemId } = useFormField();

  return (
    <FormLabel
      ref={ref}
      className={cn(error && 'text-destructive', className)}
      htmlFor={formItemId}
      {...props}
    />
  );
});
BaseLabel.displayName = 'FormLabel';

const BaseControl = forwardRef<ElementRef<typeof Slot>, ComponentPropsWithoutRef<typeof Slot>>(
  ({ ...props }, ref) => {
    const { error, formItemId, formDescriptionId, formMessageId } = useFormField();

    return (
      <Slot
        ref={ref}
        id={formItemId}
        aria-describedby={!error ? `${formDescriptionId}` : `${formDescriptionId} ${formMessageId}`}
        aria-invalid={!!error}
        {...props}
      />
    );
  }
);
BaseControl.displayName = 'FormControl';

const BaseDescription = forwardRef<HTMLParagraphElement, HTMLAttributes<HTMLParagraphElement>>(
  ({ className, ...props }, ref) => {
    const { formDescriptionId } = useFormField();

    return (
      <p
        ref={ref}
        id={formDescriptionId}
        className={cn('text-sm text-muted-foreground', className)}
        {...props}
      />
    );
  }
);
BaseDescription.displayName = 'FormDescription';

const BaseMessage = forwardRef<HTMLParagraphElement, HTMLAttributes<HTMLParagraphElement>>(
  ({ className, children, ...props }, ref) => {
    const { error, formMessageId } = useFormField();
    const body = error ? String(error?.message) : children;

    if (!body) {
      return null;
    }

    return (
      <p
        ref={ref}
        id={formMessageId}
        className={cn('text-sm font-medium text-destructive', className)}
        {...props}
      >
        {body}
      </p>
    );
  }
);
BaseMessage.displayName = 'FormMessage';

export { Form, useFormField };

interface Props<T extends FieldValues> extends UseControllerProps<T> {
  label?: string;
  description?: string;
  classNames?: ClassNames<'item' | 'label' | 'control' | 'description' | 'message'>;
  children?: ReactElement;
}

export const FormField = <T extends FieldValues>({
  control,
  name,
  label,
  description,
  classNames,
  children,
}: Props<T>) => (
  <BaseField
    control={control}
    name={name}
    render={({ field }) => (
      <BaseItem className={classNames?.item}>
        {!!label && <BaseLabel className={classNames?.label}>{label}</BaseLabel>}
        <BaseControl className={classNames?.control}>
          {children && cloneElement(children, field)}
        </BaseControl>

        {!!description && (
          <BaseDescription className={classNames?.description}>{description}</BaseDescription>
        )}
        <BaseMessage className={classNames?.message} />
      </BaseItem>
    )}
  />
);
