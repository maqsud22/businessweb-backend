import { useMemo } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Product, ProductPackage, Store } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';
import { useNavigate } from 'react-router-dom';

const lineSchema = z.object({
  productId: z.string().uuid(),
  productPackageId: z.string().uuid(),
  quantity: z.number().min(0.0001),
  unitPrice: z.number().min(0.01)
});

const schema = z.object({
  storeId: z.string().uuid(),
  paymentType: z.enum(['Cash', 'Debt', 'Partial']),
  paidAmount: z.number().min(0),
  lines: z.array(lineSchema).min(1)
});

type FormValues = z.infer<typeof schema>;

async function fetchStores(): Promise<Store[]> {
  const { data } = await apiClient.get('/api/Stores');
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

export default function SalesCreatePage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const storesQuery = useQuery({ queryKey: ['stores'], queryFn: fetchStores });
  const productsQuery = useQuery({ queryKey: ['products'], queryFn: fetchProducts });
  const packagesQuery = useQuery({ queryKey: ['productPackages'], queryFn: fetchPackages });

  const form = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      storeId: '',
      paymentType: 'Cash',
      paidAmount: 0,
      lines: [{ productId: '', productPackageId: '', quantity: 1, unitPrice: 0 }]
    }
  });

  const { fields, append, remove } = useFieldArray({
    control: form.control,
    name: 'lines'
  });

  const createMutation = useMutation({
    mutationFn: (values: FormValues) => apiClient.post('/api/Sales', values),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['sales'] });
    }
  });

  const totals = useMemo(() => {
    return form.watch('lines').reduce((sum, line) => sum + line.quantity * line.unitPrice, 0);
  }, [form.watch('lines')]);

  const onSubmit = async (values: FormValues) => {
    const response = await createMutation.mutateAsync(values);
    const saleId = response.data?.saleId;
    if (saleId) {
      navigate(`/sales/${saleId}`);
    }
  };

  return (
    <div className="space-y-4">
      <PageHeader title="Create Sale" />
      <div className="card">
        <Status loading={createMutation.isPending} error={createMutation.error?.message} />
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid gap-4 md:grid-cols-3">
            <div>
              <label className="text-sm">Store</label>
              <select className="input" {...form.register('storeId')}>
                <option value="">Select store</option>
                {storesQuery.data?.map((store) => (
                  <option key={store.id} value={store.id}>
                    {store.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="text-sm">Payment Type</label>
              <select className="input" {...form.register('paymentType')}>
                <option value="Cash">Cash</option>
                <option value="Debt">Debt</option>
                <option value="Partial">Partial</option>
              </select>
            </div>
            <div>
              <label className="text-sm">Paid Amount</label>
              <input
                type="number"
                step="0.01"
                className="input"
                {...form.register('paidAmount', { valueAsNumber: true })}
              />
            </div>
          </div>

          <div>
            <div className="mb-2 text-sm font-medium">Lines</div>
            <div className="space-y-3">
              {fields.map((field, index) => (
                <div key={field.id} className="grid gap-3 md:grid-cols-5">
                  <div>
                    <label className="text-xs">Product</label>
                    <select className="input" {...form.register(`lines.${index}.productId`)}>
                      <option value="">Select</option>
                      {productsQuery.data?.map((product) => (
                        <option key={product.id} value={product.id}>
                          {product.name}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div>
                    <label className="text-xs">Package</label>
                    <select className="input" {...form.register(`lines.${index}.productPackageId`)}>
                      <option value="">Select</option>
                      {packagesQuery.data?.map((pkg) => (
                        <option key={pkg.id} value={pkg.id}>
                          {pkg.name}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div>
                    <label className="text-xs">Qty</label>
                    <input
                      type="number"
                      step="0.0001"
                      className="input"
                      {...form.register(`lines.${index}.quantity`, { valueAsNumber: true })}
                    />
                  </div>
                  <div>
                    <label className="text-xs">Unit Price</label>
                    <input
                      type="number"
                      step="0.01"
                      className="input"
                      {...form.register(`lines.${index}.unitPrice`, { valueAsNumber: true })}
                    />
                  </div>
                  <div className="flex items-end">
                    <button
                      type="button"
                      className="button-outline w-full"
                      onClick={() => remove(index)}
                    >
                      Remove
                    </button>
                  </div>
                </div>
              ))}
              <button
                type="button"
                className="button-outline"
                onClick={() => append({ productId: '', productPackageId: '', quantity: 1, unitPrice: 0 })}
              >
                Add Line
              </button>
            </div>
          </div>

          <div className="flex items-center justify-between">
            <div className="text-sm text-slate-600">Computed Total: {totals}</div>
            <button className="button" disabled={createMutation.isPending}>
              Create Sale
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
