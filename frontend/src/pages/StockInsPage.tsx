import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Product, ProductPackage, StockIn } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

const schema = z.object({
  productId: z.string().uuid(),
  productPackageId: z.string().uuid(),
  quantity: z.number().min(1),
  costPrice: z.number().min(0),
  date: z.string().min(1)
});

type FormValues = z.infer<typeof schema>;

async function fetchStockIns(): Promise<StockIn[]> {
  const { data } = await apiClient.get('/api/StockIns');
  return data;
}

async function fetchProducts(): Promise<Product[]> {
  const { data } = await apiClient.get('/api/Products');
  return data;
}

async function fetchPackages(): Promise<ProductPackage[]> {
  const { data } = await apiClient.get('/api/ProductPackages');
  return data;
}

export default function StockInsPage() {
  const [editing, setEditing] = useState<StockIn | null>(null);
  const queryClient = useQueryClient();
  const stockQuery = useQuery({ queryKey: ['stockIns'], queryFn: fetchStockIns });
  const productsQuery = useQuery({ queryKey: ['products'], queryFn: fetchProducts });
  const packagesQuery = useQuery({ queryKey: ['productPackages'], queryFn: fetchPackages });

  const form = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { productId: '', productPackageId: '', quantity: 1, costPrice: 0, date: '' }
  });

  const createMutation = useMutation({
    mutationFn: (values: FormValues) => apiClient.post('/api/StockIns', values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['stockIns'] })
  });

  const updateMutation = useMutation({
    mutationFn: (values: FormValues & { id: string }) =>
      apiClient.put(`/api/StockIns/${values.id}`, values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['stockIns'] })
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiClient.delete(`/api/StockIns/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['stockIns'] })
  });

  const onSubmit = async (values: FormValues) => {
    if (editing) {
      await updateMutation.mutateAsync({ ...values, id: editing.id });
      setEditing(null);
    } else {
      await createMutation.mutateAsync(values);
    }
    form.reset({ productId: '', productPackageId: '', quantity: 1, costPrice: 0, date: '' });
  };

  const startEdit = (item: StockIn) => {
    setEditing(item);
    form.reset({
      productId: item.productId,
      productPackageId: item.productPackageId,
      quantity: item.quantity,
      costPrice: item.costPrice,
      date: item.date
    });
  };

  const handleDelete = (id: string) => {
    if (window.confirm('Delete stock-in record?')) {
      deleteMutation.mutate(id);
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader title="Stock In" />
      <div className="grid gap-6 md:grid-cols-[2fr,1fr]">
        <div className="card">
          <Status loading={stockQuery.isLoading} error={stockQuery.error?.message} />
          {stockQuery.data ? (
            <table className="table w-full">
              <thead className="border-b">
                <tr>
                  <th>Product</th>
                  <th>Package</th>
                  <th>Qty</th>
                  <th>Cost</th>
                  <th>Date</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {stockQuery.data.map((item) => (
                  <tr key={item.id} className="border-b last:border-0">
                    <td>{item.productName ?? item.productId}</td>
                    <td>{item.productPackageName ?? item.productPackageId}</td>
                    <td>{item.quantity}</td>
                    <td>{item.costPrice}</td>
                    <td>{item.date?.slice(0, 10)}</td>
                    <td className="text-right">
                      <button className="button-outline mr-2" onClick={() => startEdit(item)}>
                        Edit
                      </button>
                      <button className="button-outline" onClick={() => handleDelete(item.id)}>
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
          <div className="mb-2 text-sm font-medium">{editing ? 'Edit Stock In' : 'New Stock In'}</div>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-3">
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
            <div>
              <label className="text-sm">Package</label>
              <select className="input" {...form.register('productPackageId')}>
                <option value="">Select package</option>
                {packagesQuery.data?.map((pkg) => (
                  <option key={pkg.id} value={pkg.id}>
                    {pkg.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="text-sm">Quantity</label>
              <input
                type="number"
                step="1"
                className="input"
                {...form.register('quantity', { valueAsNumber: true })}
              />
            </div>
            <div>
              <label className="text-sm">Cost Price</label>
              <input
                type="number"
                step="0.01"
                className="input"
                {...form.register('costPrice', { valueAsNumber: true })}
              />
            </div>
            <div>
              <label className="text-sm">Date</label>
              <input type="date" className="input" {...form.register('date')} />
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
                  form.reset({ productId: '', productPackageId: '', quantity: 1, costPrice: 0, date: '' });
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
