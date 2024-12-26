import { forwardRef, HTMLAttributes, ReactNode } from 'react';

import { cn } from 'Utils/cn';

const Wrapper = forwardRef<HTMLDivElement, HTMLAttributes<HTMLDivElement>>(
  ({ className, ...props }, ref) => (
    <div
      ref={ref}
      className={cn('rounded-lg border bg-card p-6 text-card-foreground shadow-sm', className)}
      {...props}
    />
  )
);
Wrapper.displayName = 'CardWrapper';

const Header = forwardRef<HTMLDivElement, HTMLAttributes<HTMLDivElement>>(
  ({ className, ...props }, ref) => (
    <div ref={ref} className={cn('flex flex-col space-y-1.5', className)} {...props} />
  )
);
Header.displayName = 'CardHeader';

const Title = forwardRef<HTMLParagraphElement, HTMLAttributes<HTMLHeadingElement>>(
  ({ className, ...props }, ref) => (
    <h3
      ref={ref}
      className={cn('text-2xl font-semibold leading-none tracking-tight', className)}
      {...props}
    />
  )
);
Title.displayName = 'CardTitle';

const Description = forwardRef<HTMLParagraphElement, HTMLAttributes<HTMLParagraphElement>>(
  ({ className, ...props }, ref) => (
    <p ref={ref} className={cn('text-sm text-muted-foreground', className)} {...props} />
  )
);
Description.displayName = 'CardDescription';

const Content = forwardRef<HTMLDivElement, HTMLAttributes<HTMLDivElement>>(
  ({ className, ...props }, ref) => (
    <div ref={ref} className={cn('pb-6 pt-0', className)} {...props} />
  )
);
Content.displayName = 'CardContent';

const Footer = forwardRef<HTMLDivElement, HTMLAttributes<HTMLDivElement>>(
  ({ className, ...props }, ref) => (
    <div ref={ref} className={cn('flex items-center pt-0', className)} {...props} />
  )
);
Footer.displayName = 'CardFooter';

interface Props {
  title?: ReactNode;
  description?: ReactNode;
  classNames?: ClassNames<'wrapper' | 'headerContent' | 'content' | 'footerContent'>;
  header?: ReactNode;
  headerContent?: ReactNode;
  footer?: ReactNode;
  footerContent?: ReactNode;
  onWrapperClick?: () => void;
  children?: ReactNode;
}

export const Card = ({
  title,
  description,
  classNames,
  header,
  headerContent,
  footer,
  footerContent,
  onWrapperClick,
  children,
}: Props) => (
  <Wrapper onClick={onWrapperClick} className={classNames?.wrapper}>
    {header}
    {(!!title || !!description || !!headerContent) && (
      <Header className={classNames?.headerContent}>
        {title && <Title>{title}</Title>}
        {description && <Description>{description}</Description>}
        {headerContent}
      </Header>
    )}
    {!!children && <Content className={classNames?.content}>{children}</Content>}
    {!!footerContent && <Footer className={classNames?.footerContent}>{footerContent}</Footer>}
    {footer}
  </Wrapper>
);
