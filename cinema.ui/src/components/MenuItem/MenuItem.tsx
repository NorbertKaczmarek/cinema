import { cloneElement, ReactElement } from 'react';

import { NavigateFunction } from 'react-router-dom';

import { Button, ButtonVariant } from 'Components/Button';
import { Tooltip } from 'Components/Tooltip';
import { cn } from 'Utils/cn';

interface Props {
  label: string;
  href: string;
  classNames?: ClassNames<'button' | 'icon' | 'label'> & {
    tooltip: ClassNames<'trigger' | 'icon' | 'content' | 'text'>;
  };
  icon?: ReactElement;
  isActive?: boolean;
  isOpen?: boolean;
  onClick?: NavigateFunction;
}

export const MenuItem = ({ label, href, classNames, icon, isOpen, isActive, onClick }: Props) => (
  <Tooltip
    side="right"
    trigger={
      <Button
        variant={isActive ? ButtonVariant.Secondary : ButtonVariant.Ghost}
        className={cn(
          'mb-1 flex h-10 w-full flex-row justify-start px-3',
          !isOpen && 'py-0',
          classNames?.button
        )}
        onClick={() => onClick?.(href)}
      >
        <span className={cn('w-6', isOpen && 'mr-4')}>
          {icon &&
            cloneElement(icon, {
              className: cn('h-6 w-6', classNames?.icon),
            })}
        </span>
        {isOpen && <p className={cn('max-w-[200px] truncate', classNames?.label)}>{label}</p>}
      </Button>
    }
    classNames={classNames?.tooltip}
    asChild
  >
    {!isOpen && label}
  </Tooltip>
);
