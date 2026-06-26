import { NavLink } from 'react-router-dom';
import {
  Package,
  DollarSign,
  MapPin,
  CreditCard,
  Ruler,
  FolderOpen,
  LayoutDashboard,
} from 'lucide-react';
import { useUIStore } from '@/stores/uiStore';
import { useAuthStore } from '@/stores/authStore';

const masterDataLinks = [
  { to: '/app/master-data/materials', icon: Package, label: 'Materials' },
  { to: '/app/master-data/currencies', icon: DollarSign, label: 'Currencies' },
  { to: '/app/master-data/locations', icon: MapPin, label: 'Locations' },
  { to: '/app/master-data/payment-terms', icon: CreditCard, label: 'Payment Terms' },
  { to: '/app/master-data/uoms', icon: Ruler, label: 'Units of Measure' },
  { to: '/app/master-data/material-categories', icon: FolderOpen, label: 'Material Categories' },
];

export const Sidebar = () => {
  const { sidebarOpen } = useUIStore();
  const { user } = useAuthStore();
  const isSuperAdmin = user?.role === 'super_admin';

  if (!sidebarOpen) return null;

  return (
    <aside className="w-64 border-r bg-card flex flex-col">
      <div className="p-4 border-b">
        <span className="font-bold text-lg">ProcureHub</span>
      </div>
      <nav className="flex-1 overflow-y-auto p-3 space-y-1">
        <NavLink
          to="/app/dashboard"
          className={({ isActive }) =>
            `flex items-center gap-2 px-3 py-2 rounded-md text-sm ${
              isActive ? 'bg-primary text-primary-foreground' : 'hover:bg-muted'
            }`
          }
        >
          <LayoutDashboard className="h-4 w-4" /> Dashboard
        </NavLink>
        {isSuperAdmin && (
          <div className="pt-2">
            <p className="px-3 py-1 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Master Data
            </p>
            {masterDataLinks.map(({ to, icon: Icon, label }) => (
              <NavLink
                key={to}
                to={to}
                className={({ isActive }) =>
                  `flex items-center gap-2 px-3 py-2 rounded-md text-sm ${
                    isActive ? 'bg-primary text-primary-foreground' : 'hover:bg-muted'
                  }`
                }
              >
                <Icon className="h-4 w-4" /> {label}
              </NavLink>
            ))}
          </div>
        )}
      </nav>
    </aside>
  );
};
