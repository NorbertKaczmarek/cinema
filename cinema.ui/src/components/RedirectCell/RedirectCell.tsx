import { FC, ReactNode } from 'react';

import { useNavigate } from 'react-router-dom';

interface Props {
  path: string;
  children?: ReactNode;
}

export const RedirectCell: FC<Props> = ({ path, children }) => {
  const navigate = useNavigate();

  return (
    <div className="cursor-pointer text-blue-500" onClick={() => navigate(path)}>
      {children}
    </div>
  );
};
