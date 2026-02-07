import { useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Debt } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

async function fetchDebt(id: string): Promise<Debt> {
  const { data } = await apiClient.get(`/api/Debts/${id}`);
  return data;
}

export default function DebtDetailPage() {
  const { id } = useParams();
  const { data, isLoading, error } = useQuery({
    queryKey: ['debts', id],
    queryFn: () => fetchDebt(id || ''),
    enabled: !!id
  });

  return (
    <div className="space-y-4">
      <PageHeader title="Debt Detail" />
      <div className="card">
        <Status loading={isLoading} error={error?.message} />
        {data ? (
          <div className="space-y-4">
            <div className="grid gap-4 md:grid-cols-3">
              <div>
                <div className="text-sm text-slate-500">Debt ID</div>
                <div className="font-mono text-xs">{data.id}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Sale ID</div>
                <div className="font-mono text-xs">{data.saleId}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Status</div>
                <div>{data.isClosed ? 'Closed' : 'Open'}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Total</div>
                <div>{data.total}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Paid</div>
                <div>{data.paid}</div>
              </div>
            </div>
            <div>
              <div className="mb-2 text-sm font-medium">Payments</div>
              <table className="table w-full">
                <thead className="border-b">
                  <tr>
                    <th>ID</th>
                    <th>Amount</th>
                    <th>Date</th>
                  </tr>
                </thead>
                <tbody>
                  {data.payments?.map((payment) => (
                    <tr key={payment.id}>
                      <td className="font-mono text-xs">{payment.id.slice(0, 8)}</td>
                      <td>{payment.amount}</td>
                      <td>{payment.date?.slice(0, 10)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        ) : null}
      </div>
    </div>
  );
}
