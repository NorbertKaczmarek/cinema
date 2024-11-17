import { cloneElement, ReactElement, ReactNode } from 'react';

import { ChevronLeft, PanelsTopLeft } from 'lucide-react';

import { Button, ButtonVariant } from 'Components/Button';
import { Menu } from 'Components/Menu';
import { useSidebarContext } from 'Hooks/useSidebarContext';
import { MenuGroup } from 'Types/menu';
import { cn } from 'Utils/cn';

interface Props {
  header?: ReactNode;
  icon?: ReactElement;
  menus?: MenuGroup[];
}

export const Sidebar = ({ header, icon, menus }: Props) => {
  const { isSidebarOpen: isOpen, toggleSidebar: onToggle } = useSidebarContext();

  return (
    <aside
      className={cn(
        'fixed left-0 top-0 z-20 h-screen -translate-x-full transition-[width] duration-300 ease-in-out lg:translate-x-0',
        isOpen ? 'w-72' : 'w-[90px]'
      )}
    >
      <div className="invisible absolute -right-[16px] top-[12px] z-20 lg:visible">
        <Button
          onClick={onToggle}
          className="h-8 w-8 rounded-md p-0"
          variant={ButtonVariant.Outline}
          isButtonIcon
          icon={
            <ChevronLeft
              className={cn(
                'h-4 w-4 transition-transform duration-700 ease-in-out',
                isOpen ? 'rotate-0' : 'rotate-180'
              )}
            />
          }
        />
      </div>
      <div className="relative flex h-full flex-col overflow-y-auto px-3 py-4 shadow-md dark:shadow-zinc-800">
        <Button
          className={cn(
            'mb-1 flex flex-row transition-transform duration-300 ease-in-out',
            isOpen ? 'translate-x-0' : 'translate-x-1'
          )}
          variant={ButtonVariant.Link}
        >
          {cloneElement(icon || <PanelsTopLeft />, {
            className: 'mr-1 h-6 w-6',
          })}
          <h1
            className={cn(
              'whitespace-nowrap text-lg font-bold transition-[transform,opacity,display] duration-300 ease-in-out',
              isOpen ? 'translate-x-0 opacity-100' : 'hidden -translate-x-96 opacity-0'
            )}
          >
            {header}
          </h1>
        </Button>
        <Menu menus={menus} isOpen={isOpen} />
      </div>
    </aside>
  );
};
