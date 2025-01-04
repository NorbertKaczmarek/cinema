import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

type AuthState = {
  userId: Nullable<string>;
  token: Nullable<string>;
  role: Nullable<'Admin' | 'User'>;
  setToken: (token: string, role: 'Admin' | 'User', userId: string) => void;
  clearToken: () => void;
};

export const useAuthStore = create<AuthState>()(
  devtools(
    set => ({
      userId: localStorage.getItem('authUserId'),
      token: localStorage.getItem('authToken'),
      role: localStorage.getItem('authRole') as Nullable<'Admin' | 'User'>,
      setToken: (token, role, userId) => {
        localStorage.setItem('authToken', token);
        localStorage.setItem('authRole', role);
        localStorage.setItem('authUserId', userId);
        set({ token, role, userId });
      },
      clearToken: () => {
        localStorage.removeItem('authToken');
        localStorage.removeItem('authRole');
        localStorage.removeItem('authUserId');
        set({ userId: null, token: null, role: null });
      },
    }),
    { name: 'AuthStore' }
  )
);
