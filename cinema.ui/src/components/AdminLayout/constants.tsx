import { Clapperboard, LogOut, PanelsTopLeft, UsersIcon } from 'lucide-react';

import { MOVIES } from 'Api/paths/movies.ts';

export const adminSidebarMenus = [
  {
    label: 'System',
    items: [
      {
        label: 'Filmy',
        href: MOVIES.PRIVATE.MOVIES_TABLE,
        icon: <Clapperboard />,
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
