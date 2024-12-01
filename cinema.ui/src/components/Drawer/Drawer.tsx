import { ComponentPropsWithoutRef, ElementRef, forwardRef, HTMLAttributes, ReactNode } from 'react';
import * as DrawerPrimitive from '@radix-ui/react-dialog';

import { cva, type VariantProps } from 'class-variance-authority';
import { X } from 'lucide-react';

import { Button } from 'Components/Button';
import { cn } from 'Utils/cn';

const BaseWrapper = DrawerPrimitive.Root;

const BaseTrigger = DrawerPrimitive.Trigger;

const BaseClose = DrawerPrimitive.Close;

const BasePortal = DrawerPrimitive.Portal;

const BaseOverlay = forwardRef<
  ElementRef<typeof DrawerPrimitive.Overlay>,
  ComponentPropsWithoutRef<typeof DrawerPrimitive.Overlay>
>(({ className, ...props }, ref) => (
  <DrawerPrimitive.Overlay
    className={cn(
      'fixed inset-0 z-50 bg-black/80 data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0',
      className
    )}
    {...props}
    ref={ref}
  />
));
BaseOverlay.displayName = DrawerPrimitive.Overlay.displayName;

const BaseDrawerVariants = cva(
  'fixed z-50 gap-4 bg-background p-6 shadow-lg transition ease-in-out data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:duration-300 data-[state=open]:duration-500',
  {
    variants: {
      side: {
        top: 'inset-x-0 top-0 border-b data-[state=closed]:slide-out-to-top data-[state=open]:slide-in-from-top',
        bottom:
          'inset-x-0 bottom-0 border-t data-[state=closed]:slide-out-to-bottom data-[state=open]:slide-in-from-bottom',
        left: 'inset-y-0 left-0 h-full w-3/4 border-r data-[state=closed]:slide-out-to-left data-[state=open]:slide-in-from-left sm:max-w-sm',
        right:
          'inset-y-0 right-0 h-full w-3/4  border-l data-[state=closed]:slide-out-to-right data-[state=open]:slide-in-from-right sm:max-w-sm',
      },
    },
    defaultVariants: {
      side: 'right',
    },
  }
);

export interface DrawerContentProps
  extends ComponentPropsWithoutRef<typeof DrawerPrimitive.Content>,
    VariantProps<typeof BaseDrawerVariants> {}

const BaseContent = forwardRef<ElementRef<typeof DrawerPrimitive.Content>, DrawerContentProps>(
  ({ side = 'right', className, children, ...props }, ref) => (
    <BasePortal>
      <BaseOverlay />
      <DrawerPrimitive.Content
        ref={ref}
        className={cn(BaseDrawerVariants({ side }), className)}
        {...props}
      >
        {children}
        <DrawerPrimitive.Close className="absolute right-4 top-4 rounded-sm opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:pointer-events-none data-[state=open]:bg-secondary">
          <X className="h-4 w-4" />
          <span className="sr-only">Close</span>
        </DrawerPrimitive.Close>
      </DrawerPrimitive.Content>
    </BasePortal>
  )
);
BaseContent.displayName = DrawerPrimitive.Content.displayName;

const BaseHeader = ({ className, ...props }: HTMLAttributes<HTMLDivElement>) => (
  <div className={cn('flex flex-col space-y-2 text-center sm:text-left', className)} {...props} />
);
BaseHeader.displayName = 'DrawerHeader';

const BaseFooter = ({ className, ...props }: HTMLAttributes<HTMLDivElement>) => (
  <div
    className={cn('flex flex-col-reverse sm:flex-row sm:justify-end sm:space-x-2', className)}
    {...props}
  />
);
BaseFooter.displayName = 'DrawerFooter';

const BaseTitle = forwardRef<
  ElementRef<typeof DrawerPrimitive.Title>,
  ComponentPropsWithoutRef<typeof DrawerPrimitive.Title>
>(({ className, ...props }, ref) => (
  <DrawerPrimitive.Title
    ref={ref}
    className={cn('text-lg font-semibold text-foreground', className)}
    {...props}
  />
));
BaseTitle.displayName = DrawerPrimitive.Title.displayName;

const BaseDescription = forwardRef<
  ElementRef<typeof DrawerPrimitive.Description>,
  ComponentPropsWithoutRef<typeof DrawerPrimitive.Description>
>(({ className, ...props }, ref) => (
  <DrawerPrimitive.Description
    ref={ref}
    className={cn('text-sm text-muted-foreground', className)}
    {...props}
  />
));
BaseDescription.displayName = DrawerPrimitive.Description.displayName;

interface DrawerHeaderProps {
  title?: string;
  description?: string;
  header?: ReactNode;
}

interface DrawerFooterProps {
  footer?: ReactNode;
  footerContent?: ReactNode;
  isWithFooterClose?: boolean;
}

interface Props extends DrawerHeaderProps, DrawerFooterProps, DrawerContentProps {
  classNames?: ClassNames<
    'trigger' | 'title' | 'description' | 'header' | 'content' | 'footer' | 'closeButton'
  >;
  trigger: ReactNode;
  children?: ReactNode;
}

export const Drawer = forwardRef<HTMLDivElement, Props>(
  (
    {
      trigger,
      title,
      description,
      header,
      footer,
      footerContent,
      classNames,
      isWithFooterClose,
      children,
      ...props
    }: Props,
    ref
  ) => {
    const isBaseHeaderVisible = !!(title || description);
    const isBaseFooterVisible = !!(isWithFooterClose || footerContent);

    return (
      <BaseWrapper>
        <BaseTrigger asChild className={classNames?.trigger}>
          {trigger}
        </BaseTrigger>
        <BaseContent
          className={classNames?.content}
          onOpenAutoFocus={e => e.preventDefault()}
          ref={ref}
          {...props}
        >
          {!!header && header}
          {isBaseHeaderVisible && (
            <BaseHeader className={classNames?.header}>
              {!!title && <BaseTitle className={classNames?.title}>{title}</BaseTitle>}
              {description && (
                <BaseDescription className={classNames?.description}>{description}</BaseDescription>
              )}
            </BaseHeader>
          )}
          {children}
          {isBaseFooterVisible && (
            <BaseFooter className={classNames?.footer}>
              {!!footerContent && footerContent}
              {isWithFooterClose && (
                <BaseClose asChild>
                  <Button type="submit" className={classNames?.closeButton}>
                    Close
                  </Button>
                </BaseClose>
              )}
            </BaseFooter>
          )}
          {!!footer && footer}
        </BaseContent>
      </BaseWrapper>
    );
  }
);
