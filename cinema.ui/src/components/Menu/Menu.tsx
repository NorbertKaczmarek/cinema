import { Ellipsis } from 'lucide-react';

import { MenuItem } from 'Components/MenuItem';
import { ScrollArea } from 'Components/ScrollArea';
import { Tooltip } from 'Components/Tooltip';
import { MenuGroup } from 'Types/menu';
import { cn } from 'Utils/cn';

export interface MenuProps {
  menus?: MenuGroup[];
  className?: string;
  isOpen?: boolean;
}

export const Menu = ({ menus, isOpen }: MenuProps) => {
  const isClosed = isOpen === false;

  const closedLabelContent = (label?: string) =>
    isClosed && label ? (
      <Tooltip
        side="right"
        trigger={<Ellipsis className="mr-auto" />}
        classNames={{
          trigger: cn(
            'flex w-full justify-start items-center truncate mb-1',
            !isOpen && 'p-0 px-3'
          ),
        }}
        asChild={false}
      >
        <p>{label}</p>
      </Tooltip>
    ) : (
      <p className="pb-2" />
    );

  return (
    <ScrollArea className={cn('[&>div>div[style]]:!block', isOpen && 'pr-2')}>
      <nav className="h-full w-full">
        <ul className="flex min-h-[calc(100vh-132px)] flex-col items-start space-y-1 px-2 lg:min-h-[calc(100vh-104px)]">
          {menus?.map(({ label, items }, index) => (
            <li className={cn('w-full justify-start', label && 'pt-5')} key={index}>
              {(isOpen && label) || !isClosed ? (
                <p className="max-w-[248px] truncate px-4 pb-2 text-sm font-medium text-muted-foreground">
                  {label}
                </p>
              ) : (
                closedLabelContent(label)
              )}
              {items.map((item, index) => (
                <MenuItem key={index} isOpen={isOpen} {...item} />
              ))}
            </li>
          ))}
        </ul>
      </nav>
    </ScrollArea>
  );
};
