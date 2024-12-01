import {
  cloneElement,
  ComponentPropsWithoutRef,
  ElementRef,
  forwardRef,
  Fragment,
  ReactNode,
} from 'react';
import {
  Content,
  DropdownMenuArrow,
  Item,
  Label,
  Portal,
  Root,
  Separator,
  Trigger,
} from '@radix-ui/react-dropdown-menu';

import { UserIcon } from 'lucide-react';

import { Button, ButtonVariant } from 'Components/Button';
import { Tooltip } from 'Components/Tooltip';
import { MenuSubGroup } from 'Types/menu';
import { cn } from 'Utils/cn';

const BaseContent = forwardRef<
  ElementRef<typeof Content>,
  ComponentPropsWithoutRef<typeof Content>
>(({ className, sideOffset = 4, ...props }, ref) => (
  <Portal>
    <Content
      ref={ref}
      sideOffset={sideOffset}
      className={cn(
        'z-50 min-w-[8rem] overflow-hidden rounded-md border bg-popover p-1 text-popover-foreground shadow-md data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[side=bottom]:slide-in-from-top-2 data-[side=left]:slide-in-from-right-2 data-[side=right]:slide-in-from-left-2 data-[side=top]:slide-in-from-bottom-2',
        className
      )}
      {...props}
    />
  </Portal>
));

const BaseLabel = forwardRef<
  ElementRef<typeof Label>,
  ComponentPropsWithoutRef<typeof Label> & {
    inset?: boolean;
  }
>(({ className, inset, ...props }, ref) => (
  <Label
    ref={ref}
    className={cn('px-2 py-1.5 text-sm font-semibold', inset && 'pl-8', className)}
    {...props}
  />
));

const BaseSeparator = forwardRef<
  ElementRef<typeof Separator>,
  ComponentPropsWithoutRef<typeof Separator>
>(({ className, ...props }, ref) => (
  <Separator ref={ref} className={cn('-mx-1 my-1 h-px bg-muted', className)} {...props} />
));

const BaseItem = forwardRef<
  ElementRef<typeof Item>,
  ComponentPropsWithoutRef<typeof Item> & {
    inset?: boolean;
  }
>(({ className, inset, ...props }, ref) => (
  <Item
    ref={ref}
    className={cn(
      'relative flex cursor-default select-none items-center rounded-sm px-2 py-1.5 text-sm outline-none transition-colors focus:bg-accent focus:text-accent-foreground data-[disabled]:pointer-events-none data-[disabled]:opacity-50',
      inset && 'pl-8',
      className
    )}
    {...props}
  />
));

interface Props {
  placeholder?: string;
  profileMenus?: MenuSubGroup[];
  children?: ReactNode;
}

export const ProfileMenu = ({ placeholder = 'Profile menu', profileMenus }: Props) => (
  <Root>
    <Tooltip
      trigger={
        <Trigger asChild>
          <Button
            variant={ButtonVariant.Outline}
            className={cn('flex h-10 items-center justify-start rounded-full px-3')}
          >
            <UserIcon className="h-5 w-5" />
          </Button>
        </Trigger>
      }
      side="bottom"
    />
    <BaseContent side="bottom" align="end" className="w-40">
      <BaseLabel className="max-w-[190px] truncate">{placeholder}</BaseLabel>
      <BaseSeparator />

      {profileMenus?.map(({ items }, index) => (
        <Fragment key={index}>
          {items.map(({ label, href, icon, onClick }, index) => (
            <BaseItem key={`${label}-${index}`} asChild onClick={() => onClick?.(href)}>
              <p className="max-w-[180px] cursor-pointer truncate">
                {!!icon &&
                  cloneElement(icon, {
                    className: 'h-5 w-5 mr-3 text-muted-foreground',
                  })}
                {label}
              </p>
            </BaseItem>
          ))}
          {index < items.length - 1 && <BaseSeparator />}
        </Fragment>
      ))}
      <DropdownMenuArrow className="fill-border" />
    </BaseContent>
  </Root>
);
