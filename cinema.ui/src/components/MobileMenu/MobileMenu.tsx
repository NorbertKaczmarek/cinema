import { MenuIcon } from 'lucide-react';

import { Button, ButtonVariant } from 'Components/Button';
import { Drawer } from 'Components/Drawer';
import { Menu } from 'Components/Menu';
import { MenuGroup } from 'Types/menu';

export interface MobileMenuProps {
  menus?: MenuGroup[];
  className?: string;
  isOpen?: boolean;
}

export const MobileMenu = ({ menus }: MobileMenuProps) => {
  const trigger = (
    <Button className="h-8" variant={ButtonVariant.Ghost}>
      <MenuIcon size={20} />
    </Button>
  );

  return (
    <Drawer
      trigger={trigger}
      side="top"
      classNames={{
        trigger: 'lg:hidden',
        content: 'w-full px-3 h-full flex flex-col',
      }}
    >
      <Menu menus={menus} isOpen />
    </Drawer>
  );
};
