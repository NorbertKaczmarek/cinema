import { createContext, ReactNode, useContext, useEffect, useState } from 'react';

export enum Theme {
  Dark = 'dark',
  Light = 'light',
  System = 'system',
}

export type ThemeProviderProps = {
  children: ReactNode;
  defaultTheme?: Theme;
  storageKey?: string;
};

export type ThemeProviderState = {
  theme: Theme;
  setTheme: (theme: Theme) => void;
};

const INITIAL_STATE: ThemeProviderState = {
  theme: Theme.Dark,
  setTheme: () => null,
};

const ThemeProviderContext = createContext<ThemeProviderState>(INITIAL_STATE);

export function ThemeProvider({
  children,
  defaultTheme = Theme.Dark,
  storageKey = 'theme',
  ...props
}: ThemeProviderProps) {
  const [theme, setTheme] = useState<Theme>(defaultTheme);

  useEffect(() => {
    const root = window.document.documentElement;

    root.classList.remove(Theme.Light, Theme.Dark);

    if (theme === 'system') {
      const systemTheme = window.matchMedia('(prefers-color-scheme: dark)').matches
        ? Theme.Dark
        : Theme.Light;

      root.classList.add(systemTheme);
      return;
    }

    root.classList.add(theme);
  }, [theme]);

  const value = {
    theme,
    setTheme: (theme: Theme) => {
      localStorage.setItem(storageKey, theme);
      setTheme(theme);
    },
  };

  return (
    <ThemeProviderContext.Provider {...props} value={value}>
      {children}
    </ThemeProviderContext.Provider>
  );
}

export const useTheme = () => {
  const context = useContext(ThemeProviderContext);

  if (context === undefined) throw new Error('useTheme must be used within a ThemeProvider');

  return context;
};
