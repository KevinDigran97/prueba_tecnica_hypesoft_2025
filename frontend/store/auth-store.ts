import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  user: {
    username: string;
    email: string;
    roles: string[];
  } | null;
  _hasHydrated: boolean;
  setTokens: (accessToken: string, refreshToken: string) => void;
  setUser: (user: AuthState['user']) => void;
  logout: () => void;
  isAuthenticated: () => boolean;
  hasRole: (role: string) => boolean;
  setHasHydrated: (state: boolean) => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      accessToken: null,
      refreshToken: null,
      user: null,
      _hasHydrated: false,

      setHasHydrated: (state) => {
        set({
          _hasHydrated: state,
        });
      },

      setTokens: (accessToken, refreshToken) => {
        set({ accessToken, refreshToken });
        // Sincronizar con localStorage para el interceptor de axios
        if (typeof window !== 'undefined') {
          localStorage.setItem('access_token', accessToken);
        }
      },

      setUser: (user) => set({ user }),

      logout: () => {
        set({ accessToken: null, refreshToken: null, user: null });
        if (typeof window !== 'undefined') {
          localStorage.removeItem('access_token');
        }
      },

      isAuthenticated: () => {
        // Saltar validación de JWT: siempre retorna true
        return true;
      },

      hasRole: (role) => {
        const user = get().user;
        return user?.roles?.includes(role) ?? false;
      },
    }),
    {
      name: 'auth-storage',
      storage: createJSONStorage(() => localStorage),
      partialize: (state) => ({
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        user: state.user,
      }),
      onRehydrateStorage: () => (state) => {
        // Cuando se completa la hidratación, sincronizar el token con localStorage
        if (state) {
          if (state.accessToken && typeof window !== 'undefined') {
            localStorage.setItem('access_token', state.accessToken);
          }
          state.setHasHydrated(true);
        }
      },
    }
  )
);