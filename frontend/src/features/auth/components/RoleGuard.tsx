import { useAuthStore } from '@/stores/authStore';

interface Props {
  roles: string[];
  fallback?: React.ReactNode;
  children: React.ReactNode;
}

export const RoleGuard = ({ roles, fallback = null, children }: Props) => {
  const { hasAnyRole } = useAuthStore();
  return hasAnyRole(...roles) ? <>{children}</> : <>{fallback}</>;
};
