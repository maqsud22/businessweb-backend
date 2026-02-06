import { Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Sale } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

async function fetchSales(): Promise<Sale[]> {
  const { data } = await apiClient.get('/api/Sales');
  return data;
}

export default function SalesPage() {
  const { data, isLoading, error } = useQuery({ queryKey: ['sales'], queryFn: fetchSales });

  return (
    <div className="space-y-4">
      <PageHeader
        title="Sales"
        actions={
          <Link to="/sales/new" className="button">
            New Sale
          </Link>
        }
      />
      <div className="card">
        <Status loading={isLoading} error={error?.message} />
        {data ? (
          <table className="table w-full">
            <thead className="border-b">
              <tr>
                <th>Sale ID</th>
                <th>Store</th>
                <th>Payment</th>
                <th>Total</th>
                <th>Date</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {data.map((sale) => (
                <tr key={sale.id} className="border-b last:border-0">
                  <td className="font-mono text-xs">{sale.id.slice(0, 8)}</td>
                  <td className="font-mono text-xs">{sale.storeId.slice(0, 8)}</td>
                  <td>{sale.paymentType}</td>
                  <td>{sale.totalAmount}</td>
                  <td>{sale.createdAt?.slice(0, 10)}</td>
                  <td className="text-right">
                    <Link to={`/sales/${sale.id}`} className="button-outline">
                      View
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : null}
      </div>
    </div>
  );
}
