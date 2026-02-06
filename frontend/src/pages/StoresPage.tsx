import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Store } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

const schema = z.object({
  name: z.string().min(2),
  phone: z.string().min(3),
  isActive: z.boolean()
});

type FormValues = z.infer<typeof schema>;

async function fetchStores(): Promise<Store[]> {
  const { data } = await apiClient.get('/api/Stores');
  return data;
}

export default function StoresPage() {
  const [editing, setEditing] = useState<Store | null>(null);
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({ queryKey: ['stores'], queryFn: fetchStores });

  const form = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { name: '', phone: '', isActive: true }
  });

  const createMutation = useMutation({
    mutationFn: (values: FormValues) => apiClient.post('/api/Stores', values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['stores'] })
  });

  const updateMutation = useMutation({
    mutationFn: (values: FormValues & { id: string }) =>
      apiClient.put(`/api/Stores/${values.id}`, values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['stores'] })
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiClient.delete(`/api/Stores/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['stores'] })
  });

  const onSubmit = async (values: FormValues) => {
    if (editing) {
      await updateMutation.mutateAsync({ ...values, id: editing.id });
      setEditing(null);
    } else {
      await createMutation.mutateAsync(values);
    }
    form.reset({ name: '', phone: '', isActive: true });
  };

  const startEdit = (store: Store) => {
    setEditing(store);
    form.reset({ name: store.name, phone: store.phone, isActive: store.isActive });
  };

  const handleDelete = (id: string) => {
    if (window.confirm('Delete store?')) {
      deleteMutation.mutate(id);
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader title="Stores" />
      <div className="grid gap-6 md:grid-cols-[2fr,1fr]">
        <div className="card">
          <Status loading={isLoading} error={error?.message} />
          {data ? (
            <table className="table w-full">
              <thead className="border-b">
                <tr>
                  <th>Name</th>
                  <th>Phone</th>
                  <th>Status</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {data.map((store) => (
                  <tr key={store.id} className="border-b last:border-0">
                    <td>{store.name}</td>
                    <td>{store.phone}</td>
                    <td>{store.isActive ? 'Active' : 'Inactive'}</td>
                    <td className="text-right">
                      <button className="button-outline mr-2" onClick={() => startEdit(store)}>
                        Edit
                      </button>
                      <button className="button-outline" onClick={() => handleDelete(store.id)}>
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : null}
        </div>
        <div className="card">
          <div className="mb-2 text-sm font-medium">{editing ? 'Edit Store' : 'New Store'}</div>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-3">
            <div>
              <label className="text-sm">Name</label>
              <input className="input" {...form.register('name')} />
              {form.formState.errors.name ? (
                <div className="text-xs text-red-600">{form.formState.errors.name.message}</div>
              ) : null}
            </div>
            <div>
              <label className="text-sm">Phone</label>
              <input className="input" {...form.register('phone')} />
            </div>
            <div className="flex items-center gap-2">
              <input type="checkbox" {...form.register('isActive')} />
              <span className="text-sm">Active</span>
            </div>
            <button className="button w-full" disabled={form.formState.isSubmitting}>
              {editing ? 'Update' : 'Create'}
            </button>
            {editing ? (
              <button
                type="button"
                className="button-outline w-full"
                onClick={() => {
                  setEditing(null);
                  form.reset({ name: '', phone: '', isActive: true });
                }}
              >
                Cancel
              </button>
            ) : null}
          </form>
        </div>
      </div>
    </div>
  );
}
