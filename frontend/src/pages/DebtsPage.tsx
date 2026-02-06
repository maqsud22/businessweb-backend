import { Link } from 'react-router-dom';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Debt } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';

async function fetchDebts(): Promise<Debt[]> {
  const { data } = await apiClient.get('/api/Debts');
  return data;
}

const paySchema = z.object({
  debtId: z.string().uuid(),
  amount: z.number().min(0.01)
});

type PayForm = z.infer<typeof paySchema>;

export default function DebtsPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({ queryKey: ['debts'], queryFn: fetchDebts });

  const payForm = useForm<PayForm>({
    resolver: zodResolver(paySchema),
    defaultValues: { debtId: '', amount: 0 }
  });

  const payMutation = useMutation({
    mutationFn: (values: PayForm) => apiClient.post('/api/Debts/pay', values),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['debts'] })
  });

  const onPay = async (values: PayForm) => {
    await payMutation.mutateAsync(values);
    payForm.reset({ debtId: '', amount: 0 });
  };

  return (
    <div className="space-y-4">
      <PageHeader title="Debts" />
      <div className="grid gap-6 md:grid-cols-[2fr,1fr]">
        <div className="card">
          <Status loading={isLoading} error={error?.message} />
          {data ? (
            <table className="table w-full">
              <thead className="border-b">
                <tr>
                  <th>Debt ID</th>
                  <th>Total</th>
                  <th>Paid</th>
                  <th>Status</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {data.map((debt) => (
                  <tr key={debt.id} className="border-b last:border-0">
                    <td className="font-mono text-xs">{debt.id.slice(0, 8)}</td>
                    <td>{debt.total}</td>
                    <td>{debt.paid}</td>
                    <td>{debt.isClosed ? 'Closed' : 'Open'}</td>
                    <td className="text-right">
                      <Link className="button-outline" to={`/debts/${debt.id}`}>
                        View
                      </Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : null}
        </div>
        <div className="card">
          <div className="mb-2 text-sm font-medium">Pay Debt</div>
          <form onSubmit={payForm.handleSubmit(onPay)} className="space-y-3">
            <div>
              <label className="text-sm">Debt ID</label>
              <input className="input" {...payForm.register('debtId')} />
            </div>
            <div>
              <label className="text-sm">Amount</label>
              <input
                type="number"
                step="0.01"
                className="input"
                {...payForm.register('amount', { valueAsNumber: true })}
              />
            </div>
            <button className="button w-full" disabled={payMutation.isPending}>
              Pay
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
