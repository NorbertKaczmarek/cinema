import { ReactElement, ReactNode } from 'react';

import { Outlet, useNavigate } from 'react-router-dom';

import { Navbar, NavbarProps } from 'Components/Navbar';
import { Sidebar } from 'Components/Sidebar';
import { useSidebarContext } from 'Hooks/useSidebarContext';
import { MenuGroup } from 'Types/menu';
import { cn } from 'Utils/cn';

import { adminProfileMenus, adminSidebarMenus } from './constants';
import { generateMenuWithRedirect } from './utils';

interface Props extends NavbarProps {
  page?: ReactNode;
  headerContent?: ReactNode;
  headerIcon?: ReactElement;
  sidebarMenus?: MenuGroup[];
  footer?: ReactNode;
  children?: ReactNode;
}

const BaseLayout = ({
  page,
  headerContent,
  headerIcon,
  actions,
  sidebarMenus,
  profileMenus,
  footer,
  children,
}: Props) => {
  const { isSidebarOpen } = useSidebarContext();

  return (
    <>
      <Sidebar header={headerContent} icon={headerIcon} menus={sidebarMenus} />
      <main
        className={cn(
          'min-h-[calc(100vh_-_56px)] bg-zinc-50 transition-[margin-left] duration-300 ease-in-out dark:bg-zinc-900',
          isSidebarOpen ? 'lg:ml-72' : 'lg:ml-[90px]',
          footer ? 'min-h-[calc(100vh_-_56px)]' : 'min-h-[100vh]'
        )}
      >
        {page}
        {children && (
          <>
            <Navbar profileMenus={profileMenus} actions={actions} />
            <div className="px-4 pb-8 pt-8 lg:container sm:px-8 lg:mx-auto">{children}</div>
          </>
        )}
      </main>
      <footer
        className={cn(
          'transition-[margin-left] duration-300 ease-in-out',
          isSidebarOpen ? 'lg:ml-72' : 'lg:ml-[90px]'
        )}
      >
        {footer}
      </footer>
    </>
  );
};

export const AdminLayout = ({ children = <Outlet /> }: Props) => {
  const navigate = useNavigate();

  return (
    <BaseLayout
      sidebarMenus={generateMenuWithRedirect(adminSidebarMenus, navigate)}
      profileMenus={generateMenuWithRedirect(adminProfileMenus, navigate)}
    >
      {children}
    </BaseLayout>
  );
};
