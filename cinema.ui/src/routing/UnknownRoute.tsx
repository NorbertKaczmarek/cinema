import { Navigate } from 'react-router-dom';

import { BASE_ROUTES } from './routes';

export const UnknownRoute = () => {
  const { isAdmin } = { isAdmin: true }; // @TODO - add user when authorization is implemented

  if (isAdmin) {
    return <Navigate to={BASE_ROUTES.private.HOME} />;
  }

  return <Navigate to={BASE_ROUTES.public.HOME} />;
};
