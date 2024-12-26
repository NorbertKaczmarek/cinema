import { NavigateFunction } from 'react-router-dom';

import { NavItem } from 'Components/Navbar';

export const generateMenuWithRedirect = (menus: NavItem[], onNavigate: NavigateFunction) =>
  menus.map(menu => ({
    ...menu,
    onClick: onNavigate,
  }));
