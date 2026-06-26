import { useNavigate } from 'react-router-dom';
import { Button } from '@/components/ui/button';

export default function NotFoundPage() {
  const navigate = useNavigate();

  return (
    <div className="flex flex-col items-center justify-center min-h-screen gap-4">
      <h1 className="text-2xl font-bold">404 — Page Not Found</h1>
      <Button onClick={() => navigate('/app/dashboard')}>Go to Dashboard</Button>
    </div>
  );
}
