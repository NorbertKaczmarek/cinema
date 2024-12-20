import { createBrowserRouter, Outlet } from 'react-router-dom';

import { AdminLayout } from 'Components/AdminLayout';
import {
  AdminCategoryAdd,
  AdminCategoryDetails,
  AdminCategoryTable,
} from 'Pages/private/AdminCategory';
import { AdminMovieAdd, AdminMovieDetails, AdminMovieTable } from 'Pages/private/AdminMovie';
import { AdminOrderTable } from 'Pages/private/AdminOrders';
import {
  AdminScreeningAdd,
  AdminScreeningDetails,
  AdminScreeningTable,
} from 'Pages/private/AdminScreening';
import { AdminUserAdd, AdminUserDetails, AdminUserTable } from 'Pages/private/AdminUsers';
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
            children: [{ path: ROUTES.private.MOVIE.TABLE, element: <AdminMovieTable /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.MOVIE.ADD, element: <AdminMovieAdd /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.MOVIE.DETAILS, element: <AdminMovieDetails /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.CATEGORY.TABLE, element: <AdminCategoryTable /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.CATEGORY.ADD, element: <AdminCategoryAdd /> }],
          },
          {
            element: <PrivateRoute />,
            children: [
              { path: ROUTES.private.CATEGORY.DETAILS, element: <AdminCategoryDetails /> },
            ],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.SCREENING.TABLE, element: <AdminScreeningTable /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.SCREENING.ADD, element: <AdminScreeningAdd /> }],
          },
          {
            element: <PrivateRoute />,
            children: [
              { path: ROUTES.private.SCREENING.DETAILS, element: <AdminScreeningDetails /> },
            ],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.USER.TABLE, element: <AdminUserTable /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.USER.DETAILS, element: <AdminUserDetails /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.USER.ADD, element: <AdminUserAdd /> }],
          },
          {
            element: <PrivateRoute />,
            children: [{ path: ROUTES.private.ORDER.TABLE, element: <AdminOrderTable /> }],
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
