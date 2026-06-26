import { useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Plus, Pencil, Trash2 } from 'lucide-react';
import { toast } from 'sonner';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Card, CardContent } from '@/components/ui/card';
import { useAuthStore } from '@/stores/authStore';
import { materialCategoryApi, type MaterialCategoryDto } from '../api/materialCategoryApi';

export default function MaterialCategoryListPage() {
  const navigate = useNavigate();
  const qc = useQueryClient();
  const companyId = useAuthStore((s) => s.user?.companyId ?? '');

  const { data = [], isLoading } = useQuery({
    queryKey: ['material-categories', companyId],
    queryFn: () => materialCategoryApi.getAll(companyId),
    enabled: !!companyId,
  });

  const deleteMut = useMutation({
    mutationFn: materialCategoryApi.delete,
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['material-categories'] });
      toast.success('Category deleted');
    },
    onError: () => toast.error('Delete failed'),
  });

  const parentMap = Object.fromEntries(data.map((c: MaterialCategoryDto) => [c.id, c.name]));

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Material Categories</h1>
        <Button onClick={() => navigate('new')}>
          <Plus className="h-4 w-4 mr-2" />
          Add Category
        </Button>
      </div>
      <Card>
        <CardContent className="p-0">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Code</TableHead>
                <TableHead>Name</TableHead>
                <TableHead>Parent</TableHead>
                <TableHead>Strategic</TableHead>
                <TableHead>Status</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading ? (
                <TableRow>
                  <TableCell colSpan={6} className="text-center py-8 text-muted-foreground">
                    Loading...
                  </TableCell>
                </TableRow>
              ) : data.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} className="text-center py-8 text-muted-foreground">
                    No categories found
                  </TableCell>
                </TableRow>
              ) : (
                data.map((row: MaterialCategoryDto) => (
                  <TableRow key={row.id}>
                    <TableCell className="font-mono font-medium">{row.code}</TableCell>
                    <TableCell>{row.name}</TableCell>
                    <TableCell>{row.parentId ? (parentMap[row.parentId] ?? '-') : '-'}</TableCell>
                    <TableCell>
                      {row.isStrategic ? <Badge variant="outline">Strategic</Badge> : '-'}
                    </TableCell>
                    <TableCell>
                      <Badge variant={row.isActive ? 'default' : 'secondary'}>
                        {row.isActive ? 'Active' : 'Inactive'}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right space-x-2">
                      <Button variant="ghost" size="icon" onClick={() => navigate(row.id)}>
                        <Pencil className="h-4 w-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => {
                          if (confirm('Delete this category?')) deleteMut.mutate(row.id);
                        }}
                      >
                        <Trash2 className="h-4 w-4 text-destructive" />
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
