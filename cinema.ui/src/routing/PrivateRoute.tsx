import { Outlet } from 'react-router-dom';

import { UnknownRoute } from './UnknownRoute';

export const PrivateRoute = () => {
  const { isAdmin } = { isAdmin: true }; // @TODO - add user when authorization is implemented

  if (!isAdmin) {
    return <UnknownRoute />;
  }

  return <Outlet />;
};
