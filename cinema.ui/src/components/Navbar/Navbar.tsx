import { ReactElement, ReactNode } from 'react';

import { Button, ButtonVariant } from 'Components/Button';
import { ProfileMenu } from 'Components/ProfileMenu';
import { MenuGroup } from 'Types/menu';

export interface NavItem {
  label: string;
  href?: string;
  icon?: ReactElement;
  onClick?: (href?: string) => void;
}

export interface NavbarProps {
  navItems?: NavItem[];
  profileMenus?: MenuGroup[];
  actions?: ReactNode;
}

export const Navbar = ({ navItems, profileMenus, actions }: NavbarProps) => (
  <header className="sticky top-0 z-10 w-full bg-background/95 shadow backdrop-blur supports-[backdrop-filter]:bg-background/60 dark:shadow-secondary">
    <div className="flex h-14 w-full items-center justify-between px-4 lg:container sm:px-8 lg:mx-auto">
      <div className="flex items-center space-x-2 lg:space-x-0">
        {navItems?.map(({ label, href, icon, onClick }) => (
          <Button
            variant={ButtonVariant.Ghost}
            key={href}
            onClick={() => onClick?.(href)}
            icon={icon}
          >
            {label}
          </Button>
        ))}
      </div>
      <div className="flex items-center justify-end gap-4">
        {actions}
        <ProfileMenu profileMenus={profileMenus} />
      </div>
    </div>
  </header>
);
