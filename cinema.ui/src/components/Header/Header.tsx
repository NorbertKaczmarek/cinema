import { cloneElement, ReactElement, ReactNode } from 'react';

import { ChevronLeft } from 'lucide-react';

import { Button, ButtonSize, ButtonVariant } from 'Components/Button';
import { cn } from 'Utils/cn';

interface Props {
  classNames?: ClassNames<'wrapper' | 'title'>;
  title?: string;
  icon?: ReactElement;
  additionalActions?: ReactNode;
  onClick?: () => void;
}

export const Header = ({ classNames, title, icon, additionalActions, onClick }: Props) => (
  <div className={cn('flex items-center justify-between', classNames?.wrapper)}>
    <div className="flex items-baseline space-x-2.5">
      {!!onClick && (
        <Button
          size={ButtonSize.Icon}
          variant={ButtonVariant.Outline}
          icon={cloneElement(icon ?? <ChevronLeft />, {
            className: icon?.props.className,
          })}
          isButtonIcon
          onClick={onClick}
        />
      )}
      {!!title && <span className={cn('text-2xl font-normal', classNames?.title)}>{title}</span>}
    </div>
    {!!additionalActions && additionalActions}
  </div>
);
