import { LogOut, User } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { useAuthStore } from '@/stores/authStore';
import { keycloak } from '@/shared/lib/keycloak';

export const Topbar = () => {
  const { user } = useAuthStore();

  return (
    <header className="h-14 border-b bg-card flex items-center justify-between px-6">
      <div />
      <div className="flex items-center gap-3">
        <span className="text-sm text-muted-foreground flex items-center gap-1">
          <User className="h-4 w-4" />
          {user?.fullName}
        </span>
        <Button variant="ghost" size="sm" onClick={() => keycloak.logout()}>
          <LogOut className="h-4 w-4" />
        </Button>
      </div>
    </header>
  );
};
