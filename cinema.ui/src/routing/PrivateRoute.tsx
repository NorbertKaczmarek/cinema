import { Navigate, Outlet } from 'react-router-dom';

import { ROUTES } from 'Routing/routes';
import { useAuthStore } from 'Store/authStore';
import { decodeToken } from 'Utils/decode';

import { UnknownRoute } from './UnknownRoute';

type Props = {
  requiredRoles?: ('Admin' | 'User')[];
};

export const PrivateRoute = ({ requiredRoles = [] }: Props) => {
  const { token, role } = useAuthStore();

  if (!token || !role || (!!requiredRoles?.length && !requiredRoles?.includes(role))) {
    return <UnknownRoute />;
  }

  const decoded = decodeToken(token);

  if (decoded.exp && Date.now() >= decoded.exp * 1000) {
    useAuthStore.getState().clearToken();
    return <Navigate to={ROUTES.private.AUTH} replace />;
  }

  return <Outlet />;
};
