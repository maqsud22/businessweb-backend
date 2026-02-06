import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';
import { SalesSummary, StockReportItem } from '../api/types';

async function fetchDaily(): Promise<SalesSummary> {
  const { data } = await apiClient.get('/api/Reports/daily');
  return data;
}

async function fetchStock(): Promise<StockReportItem[]> {
  const { data } = await apiClient.get('/api/Reports/stock');
  return data;
}

export default function DashboardPage() {
  const dailyQuery = useQuery({ queryKey: ['reports', 'daily'], queryFn: fetchDaily });
  const stockQuery = useQuery({ queryKey: ['reports', 'stock'], queryFn: fetchStock });

  return (
    <div className="space-y-6">
      <PageHeader title="Dashboard" subtitle="Daily overview" />
      <Status loading={dailyQuery.isLoading} error={dailyQuery.error?.message} />
      {dailyQuery.data ? (
        <div className="grid gap-4 md:grid-cols-3">
          <div className="stat-card">
            <div className="text-sm text-slate-500">Total Sales</div>
            <div className="text-2xl font-semibold">{dailyQuery.data.totalSales}</div>
          </div>
          <div className="stat-card">
            <div className="text-sm text-slate-500">Cash Sales</div>
            <div className="text-2xl font-semibold">{dailyQuery.data.cashSales}</div>
          </div>
          <div className="stat-card">
            <div className="text-sm text-slate-500">Debt Payments</div>
            <div className="text-2xl font-semibold">{dailyQuery.data.debtPayments}</div>
          </div>
        </div>
      ) : null}

      <div className="card">
        <div className="mb-2 text-sm font-medium">Stock Snapshot</div>
        <Status loading={stockQuery.isLoading} error={stockQuery.error?.message} />
        {stockQuery.data ? (
          <table className="table w-full">
            <thead className="border-b">
              <tr>
                <th className="text-left">Product</th>
                <th className="text-right">Incoming</th>
                <th className="text-right">Sold</th>
                <th className="text-right">In Stock</th>
              </tr>
            </thead>
            <tbody>
              {stockQuery.data.map((item) => (
                <tr key={item.productId} className="border-b last:border-0">
                  <td>{item.productName}</td>
                  <td className="text-right">{item.incomingBaseQty}</td>
                  <td className="text-right">{item.soldBaseQty}</td>
                  <td className="text-right">{item.inStockBaseQty}</td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : null}
      </div>
    </div>
  );
}
