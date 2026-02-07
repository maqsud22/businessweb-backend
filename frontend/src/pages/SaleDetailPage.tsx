import { useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Sale } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

async function fetchSale(id: string): Promise<Sale> {
  const { data } = await apiClient.get(`/api/Sales/${id}`);
  return data;
}

export default function SaleDetailPage() {
  const { id } = useParams();
  const { data, isLoading, error } = useQuery({
    queryKey: ['sales', id],
    queryFn: () => fetchSale(id || ''),
    enabled: !!id
  });

  return (
    <div className="space-y-4">
      <PageHeader title="Sale Detail" />
      <div className="card">
        <Status loading={isLoading} error={error?.message} />
        {data ? (
          <div className="space-y-4">
            <div className="grid gap-4 md:grid-cols-3">
              <div>
                <div className="text-sm text-slate-500">Sale ID</div>
                <div className="font-mono text-xs">{data.id}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Store</div>
                <div className="font-mono text-xs">{data.storeId}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Payment</div>
                <div>{data.paymentType}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Total</div>
                <div>{data.totalAmount}</div>
              </div>
              <div>
                <div className="text-sm text-slate-500">Created</div>
                <div>{data.createdAt?.slice(0, 10)}</div>
              </div>
            </div>
            <div>
              <div className="mb-2 text-sm font-medium">Lines</div>
              <table className="table w-full">
                <thead className="border-b">
                  <tr>
                    <th>Product</th>
                    <th>Package</th>
                    <th>Qty</th>
                    <th>Unit Price</th>
                    <th>Total</th>
                  </tr>
                </thead>
                <tbody>
                  {data.lines?.map((line) => (
                    <tr key={line.id ?? `${line.productId}-${line.productPackageId}`}>
                      <td className="font-mono text-xs">{line.productId.slice(0, 8)}</td>
                      <td className="font-mono text-xs">{line.productPackageId.slice(0, 8)}</td>
                      <td>{line.quantity}</td>
                      <td>{line.unitPrice}</td>
                      <td>{line.lineTotal}</td>
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
