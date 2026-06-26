import { useEffect } from 'react';
import { keycloak } from '@/shared/lib/keycloak';

export default function LoginRedirectPage() {
  useEffect(() => {
    keycloak.login();
  }, []);

  return (
    <div className="flex items-center justify-center min-h-screen">
      <p className="text-muted-foreground">Redirecting to login...</p>
    </div>
  );
}
