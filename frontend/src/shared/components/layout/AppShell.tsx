import { Outlet } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { Topbar } from './Topbar';
import { useUIStore } from '@/stores/uiStore';
import { useAuth } from '@/features/auth/hooks/useAuth';

export const AppShell = () => {
  useAuth();
  const { sidebarOpen } = useUIStore();

  return (
    <div className="flex h-screen overflow-hidden bg-background">
      <Sidebar />
      <div
        className={`flex flex-col flex-1 overflow-hidden transition-all ${sidebarOpen ? 'ml-0' : ''}`}
      >
        <Topbar />
        <main className="flex-1 overflow-y-auto p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
};
