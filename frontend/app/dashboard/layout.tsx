"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { Sidebar } from "@/components/layout/sidebar";
import { Header } from "@/components/layout/header";
import { useAuthStore } from "@/store/auth-store";
import { Toaster } from "@/components/ui/sonner";
import { Loader2 } from "lucide-react";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const router = useRouter();
  const [isHydrated, setIsHydrated] = useState(false);
  const hasHydrated = useAuthStore((state) => state._hasHydrated);
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const setHasHydrated = useAuthStore((state) => state.setHasHydrated);

  // Esperar a que el componente se monte en el cliente y el store se hidrate
  useEffect(() => {
    // Verificar si estamos en el cliente
    if (typeof window === 'undefined') return;
    
    // Verificar si hay token en localStorage como indicador rápido
    const tokenFromStorage = localStorage.getItem('access_token');
    const authStorageData = localStorage.getItem('auth-storage');
    
    // Función para verificar y establecer hidratación
    const checkAndSetHydration = () => {
      const currentState = useAuthStore.getState();
      const hasToken = currentState.accessToken || tokenFromStorage;
      
      // Si el store ya marcó como hidratado, usar ese estado
      if (hasHydrated) {
        setIsHydrated(true);
        return;
      }
      
      // Si hay token (en el store o en localStorage), el store debería estar listo
      if (hasToken || authStorageData) {
        // Si hay datos pero el store no se ha marcado como hidratado, marcarlo
        if (!hasHydrated) {
          setHasHydrated(true);
        }
        setIsHydrated(true);
      } else {
        // No hay datos, marcar como hidratado de todas formas para continuar
        setHasHydrated(true);
        setIsHydrated(true);
      }
    };

    // Pequeño delay para permitir que Zustand termine de hidratar desde localStorage
    const timer = setTimeout(checkAndSetHydration, 50);
    return () => clearTimeout(timer);
  }, [hasHydrated, setHasHydrated]);

  // Verificar autenticación usando tanto el store como localStorage
  const tokenInStorage = typeof window !== 'undefined' 
    ? localStorage.getItem('access_token') 
    : null;
  const storeAuthenticated = isAuthenticated();
  const isAuth = storeAuthenticated || !!tokenInStorage;

  useEffect(() => {
    // Solo verificar autenticación después de que el store se haya hidratado
    if (!isHydrated) return;

    // Si no hay autenticación después de la hidratación, redirigir
    if (!isAuth) {
      router.push("/auth/login");
    }
  }, [isHydrated, isAuth, router]);

  // Mostrar loading mientras se hidrata el store
  if (!isHydrated) {
    return (
      <div className="flex h-screen items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  // Si está hidratado pero no autenticado, mostrar loading (mientras redirige)
  if (!isAuth) {
    return (
      <div className="flex h-screen items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  return (
    <div className="flex h-screen overflow-hidden">

      <aside className="hidden w-64 lg:block">
        <Sidebar />
      </aside>

      {/* Main Content */}
      <div className="flex flex-1 flex-col overflow-hidden">
        <Header />
        <main className="flex-1 overflow-y-auto bg-background p-6">
          {children}
        </main>
      </div>

      <Toaster />
    </div>
  );
}