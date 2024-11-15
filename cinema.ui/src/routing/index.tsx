import { createBrowserRouter, Outlet } from 'react-router-dom';

import { AdminLayout } from 'Components/AdminLayout';
import { AdminMovieAdd, AdminMovieDetails, AdminMovieTable } from 'Pages/private';
import { MockHome } from 'Routing/MockHome';
import { ROUTES } from 'Routing/routes';

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
            children: [{ path: ROUTES.private.MOVIES_TABLE, element: <AdminMovieTable /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.ADD_MOVIE, element: <AdminMovieAdd /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.MOVIE_DETAILS, element: <AdminMovieDetails /> }],
          },
        ],
      },
      {
        path: '',
        element: <MockHome />,
      },
      {
        path: '*',
        element: <UnknownRoute />,
      },
    ],
  },
]);
