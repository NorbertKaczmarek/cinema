import { ReactNode, useEffect } from 'react';

import { Outlet, useLocation, useNavigate } from 'react-router-dom';

import { Navbar, NavbarProps } from 'Components/Navbar';
import { userNavItems } from 'Components/UserLayout/constants';
import { generateMenuWithRedirect } from 'Components/UserLayout/utils';
import { Theme, ThemeProvider, useTheme } from 'Hooks/useTheme';
import { cn } from 'Utils/cn';

interface Props extends NavbarProps {
  page?: ReactNode;
  footer?: ReactNode;
  children?: ReactNode;
}

const BaseLayout = ({ page, actions, navItems, children }: Props) => {
  return (
    <main
      className={cn(
        'min-h-[100vh] bg-zinc-50 transition-[margin-left] duration-300 ease-in-out dark:bg-zinc-900'
      )}
    >
      {page}
      {children && (
        <>
          <Navbar navItems={navItems} actions={actions} />
          <div className="px-4 pb-8 lg:container sm:px-8 lg:mx-auto">{children}</div>
        </>
      )}
    </main>
  );
};

export const UserLayout = ({ children = <Outlet /> }: Props) => {
  const navigate = useNavigate();

  const { pathname } = useLocation();

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  return (
    <ThemeProvider defaultTheme={Theme.Dark}>
      <BaseLayout navItems={generateMenuWithRedirect(userNavItems, navigate)}>
        {children}
      </BaseLayout>
    </ThemeProvider>
  );
};
