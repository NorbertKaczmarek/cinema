import { Navigate } from 'react-router-dom';

import { ROUTES } from './routes';

export const UnknownRoute = () => {
  const { isAdmin } = { isAdmin: true }; // @TODO - add user when authorization is implemented

  if (isAdmin) {
    return <Navigate to={ROUTES.private.HOME} />;
  }

  return <Navigate to={ROUTES.public.HOME} />;
};
