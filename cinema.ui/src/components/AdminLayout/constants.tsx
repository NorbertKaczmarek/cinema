import {
  Clapperboard,
  ListIcon,
  LogOut,
  PanelsTopLeft,
  ProjectorIcon,
  UsersIcon,
} from 'lucide-react';

import { ROUTES } from 'Routing/routes';

export const adminSidebarMenus = [
  {
    label: 'System',
    items: [
      {
        label: 'Filmy',
        href: ROUTES.private.MOVIE.TABLE,
        icon: <Clapperboard />,
      },
      {
        label: 'Kategorie',
        href: ROUTES.private.CATEGORY.TABLE,
        icon: <ListIcon />,
      },
      {
        label: 'Seanse',
        href: ROUTES.private.SCREENING.TABLE,
        icon: <ProjectorIcon />,
      },
    ],
  },
];

export const adminProfileMenus = [
  {
    items: [
      {
        label: 'Account',
        href: '/account',
        icon: <UsersIcon />,
      },
      {
        label: 'Dashboard',
        href: '/dashboard',
        icon: <PanelsTopLeft />,
      },
    ],
  },
  {
    items: [
      {
        label: 'Sign Out',
        href: '/signOut',
        icon: <LogOut />,
      },
    ],
  },
];
