import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Product } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

const schema = z.object({
  name: z.string().min(2),
  unit: z.number().min(0),
  defaultPrice: z.number().min(0)
});

type FormValues = z.infer<typeof schema>;

async function fetchProducts(): Promise<Product[]> {
  const { data } = await apiClient.get('/api/Products');
  return data;
}

export default function ProductsPage() {
  const [editing, setEditing] = useState<Product | null>(null);
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({ queryKey: ['products'], queryFn: fetchProducts });

  const form = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { name: '', unit: 0, defaultPrice: 0 }
  });

  const createMutation = useMutation({
    mutationFn: (values: FormValues) => apiClient.post('/api/Products', values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['products'] })
  });

  const updateMutation = useMutation({
    mutationFn: (values: FormValues & { id: string }) =>
      apiClient.put(`/api/Products/${values.id}`, values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['products'] })
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiClient.delete(`/api/Products/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['products'] })
  });

  const onSubmit = async (values: FormValues) => {
    if (editing) {
      await updateMutation.mutateAsync({ ...values, id: editing.id });
      setEditing(null);
    } else {
      await createMutation.mutateAsync(values);
    }
    form.reset({ name: '', unit: 0, defaultPrice: 0 });
  };

  const startEdit = (product: Product) => {
    setEditing(product);
    form.reset({ name: product.name, unit: product.unit, defaultPrice: product.defaultPrice });
  };

  const handleDelete = (id: string) => {
    if (window.confirm('Delete product?')) {
      deleteMutation.mutate(id);
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader title="Products" />
      <div className="grid gap-6 md:grid-cols-[2fr,1fr]">
        <div className="card">
          <Status loading={isLoading} error={error?.message} />
          {data ? (
            <table className="table w-full">
              <thead className="border-b">
                <tr>
                  <th>Name</th>
                  <th>Unit</th>
                  <th>Default Price</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {data.map((product) => (
                  <tr key={product.id} className="border-b last:border-0">
                    <td>{product.name}</td>
                    <td>{product.unit}</td>
                    <td>{product.defaultPrice}</td>
                    <td className="text-right">
                      <button className="button-outline mr-2" onClick={() => startEdit(product)}>
                        Edit
                      </button>
                      <button className="button-outline" onClick={() => handleDelete(product.id)}>
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
          <div className="mb-2 text-sm font-medium">{editing ? 'Edit Product' : 'New Product'}</div>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-3">
            <div>
              <label className="text-sm">Name</label>
              <input className="input" {...form.register('name')} />
            </div>
            <div>
              <label className="text-sm">Unit (enum value)</label>
              <input type="number" className="input" {...form.register('unit', { valueAsNumber: true })} />
            </div>
            <div>
              <label className="text-sm">Default Price</label>
              <input
                type="number"
                step="0.01"
                className="input"
                {...form.register('defaultPrice', { valueAsNumber: true })}
              />
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
                  form.reset({ name: '', unit: 0, defaultPrice: 0 });
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
