import {
  CalendarDays,
  Clapperboard,
  ListIcon,
  LogOut,
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
      {
        label: 'Pracownicy',
        href: ROUTES.private.USER.TABLE,
        icon: <UsersIcon />,
      },
      {
        label: 'Zamówienia',
        href: ROUTES.private.ORDER.TABLE,
        icon: <CalendarDays />,
      },
    ],
  },
];

export const getAdminProfileMenus = (userId: string) => [
  {
    items: [
      {
        label: 'Profil',
        href: `/admin/users/${userId}`,
        icon: <UsersIcon />,
      },
    ],
  },
  {
    items: [
      {
        label: 'Wyloguj się',
        href: '/signOut',
        icon: <LogOut />,
      },
    ],
  },
];
