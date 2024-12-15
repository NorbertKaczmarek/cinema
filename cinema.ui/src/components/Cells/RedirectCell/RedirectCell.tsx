import { ReactNode } from 'react';

import { useNavigate } from 'react-router-dom';

interface Props {
  route: string;
  children?: ReactNode;
}

export const RedirectCell = ({ route, children }: Props) => {
  const navigate = useNavigate();

  return (
    <div className="cursor-pointer text-blue-500" onClick={() => navigate(route)}>
      {children}
    </div>
  );
};
