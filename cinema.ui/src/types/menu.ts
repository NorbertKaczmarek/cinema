import { ReactElement } from 'react';

import { NavigateFunction } from 'react-router-dom';

export interface MenuItemProps {
  label: string;
  href: string;
  icon?: ReactElement;
  isActive?: boolean;
  isDisabled?: boolean;
  onClick?: NavigateFunction;
  onLogout?: () => void;
}

export interface MenuGroup {
  label?: string;
  items: MenuItemProps[];
}

export interface MenuSubGroup {
  label?: string;
  items: MenuItemProps[];
}
