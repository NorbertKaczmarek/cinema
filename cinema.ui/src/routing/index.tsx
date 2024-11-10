import { createBrowserRouter, Outlet } from 'react-router-dom';

import { MOVIES } from 'Api/paths/movies.ts';
import { AdminLayout } from 'Components/AdminLayout';
import { AdminMovieTable } from 'Pages/private';

import { PrivateRoute } from './PrivateRoute';
import { UnknownRoute } from './UnknownRoute';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <Outlet />,
    children: [
      {
        path: '/admin',
        element: <AdminLayout />,
        children: [
          {
            element: <PrivateRoute />,
            children: [{ path: MOVIES.PRIVATE.MOVIES_TABLE, element: <AdminMovieTable /> }],
          },
        ],
      },
      {
        path: '*',
        element: <UnknownRoute />,
      },
    ],
  },
]);
