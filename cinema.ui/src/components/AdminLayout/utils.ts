import { NavigateFunction } from 'react-router-dom';

import { MenuGroup } from 'Types/menu';

export const generateMenuWithRedirect = (menus: MenuGroup[], onNavigate: NavigateFunction) =>
  menus.map(menu => ({
    ...menu,
    items: menu.items?.map(item => ({
      ...item,
      onClick: onNavigate,
    })),
  }));
