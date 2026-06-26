import { useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { keycloak } from '@/shared/lib/keycloak';
import { useAuthStore } from '@/stores/authStore';
import { apiClient } from '@/shared/lib/axios';

export const useAuth = () => {
  const navigate = useNavigate();
  const { setUser, clearUser, setInitialized, user } = useAuthStore();
  const initialized = useRef(false);

  useEffect(() => {
    if (initialized.current) return;
    initialized.current = true;

    keycloak
      .init({ onLoad: 'check-sso', pkceMethod: 'S256' })
      .then(async (authenticated) => {
        if (authenticated) {
          try {
            const res = await apiClient.get('/auth/profile');
            setUser(res.data.data);
            const role = res.data.data.role;
            const p = window.location.pathname;
            if (p === '/' || p === '/login') {
              if (role === 'approver') navigate('/app/approval/inbox');
              else if (role === 'management') navigate('/app/analytics');
              else if (role === 'vendor_admin' || role === 'vendor_staff')
                navigate('/vendor/dashboard');
              else navigate('/app/dashboard');
            }
          } catch {
            clearUser();
          }
        }
        setInitialized(true);
      })
      .catch(() => setInitialized(true));
  }, []);

  return { user, isAuthenticated: !!keycloak.authenticated };
};
