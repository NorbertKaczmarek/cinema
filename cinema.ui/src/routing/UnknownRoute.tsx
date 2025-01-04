import { Navigate } from 'react-router-dom';

import { useAuthStore } from 'Store/authStore';

import { ROUTES } from './routes';

export const UnknownRoute = () => {
  const { token } = useAuthStore();

  if (token) {
    return <Navigate to={ROUTES.private.ORDER.TABLE} />;
  }

  return <Navigate to={ROUTES.public.HOME} />;
};
