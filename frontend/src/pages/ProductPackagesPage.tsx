import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Product, ProductPackage } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

const schema = z.object({
  name: z.string().min(2),
  multiplier: z.number().min(0.0001),
  productId: z.string().uuid()
});

type FormValues = z.infer<typeof schema>;

async function fetchPackages(): Promise<ProductPackage[]> {
  const { data } = await apiClient.get('/api/ProductPackages');
  return data;
}

async function fetchProducts(): Promise<Product[]> {
  const { data } = await apiClient.get('/api/Products');
  return data;
}

export default function ProductPackagesPage() {
  const [editing, setEditing] = useState<ProductPackage | null>(null);
  const queryClient = useQueryClient();
  const packagesQuery = useQuery({ queryKey: ['productPackages'], queryFn: fetchPackages });
  const productsQuery = useQuery({ queryKey: ['products'], queryFn: fetchProducts });

  const form = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { name: '', multiplier: 1, productId: '' }
  });

  const createMutation = useMutation({
    mutationFn: (values: FormValues) => apiClient.post('/api/ProductPackages', values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['productPackages'] })
  });

  const updateMutation = useMutation({
    mutationFn: (values: FormValues & { id: string }) =>
      apiClient.put(`/api/ProductPackages/${values.id}`, values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['productPackages'] })
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiClient.delete(`/api/ProductPackages/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['productPackages'] })
  });

  const onSubmit = async (values: FormValues) => {
    if (editing) {
      await updateMutation.mutateAsync({ ...values, id: editing.id });
      setEditing(null);
    } else {
      await createMutation.mutateAsync(values);
    }
    form.reset({ name: '', multiplier: 1, productId: '' });
  };

  const startEdit = (item: ProductPackage) => {
    setEditing(item);
    form.reset({ name: item.name, multiplier: item.multiplier, productId: item.productId });
  };

  const handleDelete = (id: string) => {
    if (window.confirm('Delete package?')) {
      deleteMutation.mutate(id);
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader title="Product Packages" />
      <div className="grid gap-6 md:grid-cols-[2fr,1fr]">
        <div className="card">
          <Status loading={packagesQuery.isLoading} error={packagesQuery.error?.message} />
          {packagesQuery.data ? (
            <table className="table w-full">
              <thead className="border-b">
                <tr>
                  <th>Name</th>
                  <th>Multiplier</th>
                  <th>Product</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {packagesQuery.data.map((pkg) => (
                  <tr key={pkg.id} className="border-b last:border-0">
                    <td>{pkg.name}</td>
                    <td>{pkg.multiplier}</td>
                    <td>{pkg.productName ?? pkg.productId}</td>
                    <td className="text-right">
                      <button className="button-outline mr-2" onClick={() => startEdit(pkg)}>
                        Edit
                      </button>
                      <button className="button-outline" onClick={() => handleDelete(pkg.id)}>
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
          <div className="mb-2 text-sm font-medium">{editing ? 'Edit Package' : 'New Package'}</div>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-3">
            <div>
              <label className="text-sm">Name</label>
              <input className="input" {...form.register('name')} />
            </div>
            <div>
              <label className="text-sm">Multiplier</label>
              <input
                type="number"
                step="0.0001"
                className="input"
                {...form.register('multiplier', { valueAsNumber: true })}
              />
            </div>
            <div>
              <label className="text-sm">Product</label>
              <select className="input" {...form.register('productId')}>
                <option value="">Select product</option>
                {productsQuery.data?.map((product) => (
                  <option key={product.id} value={product.id}>
                    {product.name}
                  </option>
                ))}
              </select>
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
                  form.reset({ name: '', multiplier: 1, productId: '' });
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
