import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Product, ProductReport, SalesSummary, StockReportItem } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';
import { formatNumber } from '../lib/format';

async function fetchDaily(date?: string): Promise<SalesSummary> {
  const { data } = await apiClient.get('/api/Reports/daily', { params: { date } });
  return data;
}

async function fetchMonthly(date?: string): Promise<SalesSummary> {
  const { data } = await apiClient.get('/api/Reports/monthly', { params: { date } });
  return data;
}

async function fetchStock(): Promise<StockReportItem[]> {
  const { data } = await apiClient.get('/api/Reports/stock');
  return data;
}

async function fetchProducts(): Promise<Product[]> {
  const { data } = await apiClient.get('/api/Products');
  return data;
}

async function fetchProductReport(id: string): Promise<ProductReport> {
  const { data } = await apiClient.get(`/api/Reports/product/${id}`);
  return data;
}

export default function ReportsPage() {
  const [dailyDate, setDailyDate] = useState('');
  const [monthlyDate, setMonthlyDate] = useState('');
  const [productId, setProductId] = useState('');

  const dailyQuery = useQuery({
    queryKey: ['reports', 'daily', dailyDate],
    queryFn: () => fetchDaily(dailyDate || undefined)
  });

  const monthlyQuery = useQuery({
    queryKey: ['reports', 'monthly', monthlyDate],
    queryFn: () => fetchMonthly(monthlyDate || undefined)
  });

  const stockQuery = useQuery({ queryKey: ['reports', 'stock'], queryFn: fetchStock });
  const productsQuery = useQuery({ queryKey: ['products'], queryFn: fetchProducts });
  const productReportQuery = useQuery({
    queryKey: ['reports', 'product', productId],
    queryFn: () => fetchProductReport(productId),
    enabled: !!productId
  });

  const selectedProduct = productsQuery.data?.find((product) => product.id === productId);

  const handleDownloadProductReport = () => {
    if (!productReportQuery.data || !selectedProduct) return;

    const rows = [
      ['Product', selectedProduct.name],
      ['Incoming', formatNumber(productReportQuery.data.incomingBaseQty)],
      ['Sold', formatNumber(productReportQuery.data.soldBaseQty)],
      ['In Stock', formatNumber(productReportQuery.data.inStockBaseQty)]
    ];

    const csv = rows.map((row) => row.map((cell) => `"${cell}"`).join(',')).join('\n');
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `product-report-${selectedProduct.name.replace(/\s+/g, '-').toLowerCase()}.csv`;
    link.click();
    URL.revokeObjectURL(url);
  };

  return (
    <div className="space-y-6">
      <PageHeader title="Reports" />
      <div className="grid gap-6 md:grid-cols-2">
        <div className="card space-y-3">
          <div className="text-sm font-medium">Daily Report</div>
          <input
            type="date"
            className="input"
            value={dailyDate}
            onChange={(e) => setDailyDate(e.target.value)}
          />
          <Status loading={dailyQuery.isLoading} error={dailyQuery.error?.message} />
          {dailyQuery.data ? (
            <div className="text-sm text-slate-700">
              Total: <span className="number-strong">{formatNumber(dailyQuery.data.totalSales)}</span> | Cash:{' '}
              <span className="number-strong">{formatNumber(dailyQuery.data.cashSales)}</span> | Debt:{' '}
              <span className="number-strong">{formatNumber(dailyQuery.data.debtSales)}</span>
            </div>
          ) : null}
        </div>
        <div className="card space-y-3">
          <div className="text-sm font-medium">Monthly Report</div>
          <input
            type="date"
            className="input"
            value={monthlyDate}
            onChange={(e) => setMonthlyDate(e.target.value)}
          />
          <Status loading={monthlyQuery.isLoading} error={monthlyQuery.error?.message} />
          {monthlyQuery.data ? (
            <div className="text-sm text-slate-700">
              Total: <span className="number-strong">{formatNumber(monthlyQuery.data.totalSales)}</span> | Cash:{' '}
              <span className="number-strong">{formatNumber(monthlyQuery.data.cashSales)}</span> | Debt:{' '}
              <span className="number-strong">{formatNumber(monthlyQuery.data.debtSales)}</span>
            </div>
          ) : null}
        </div>
      </div>

      <div className="card space-y-3">
        <div className="flex flex-wrap items-center justify-between gap-2">
          <div className="text-sm font-medium">Product Report</div>
          <button
            type="button"
            className="button-outline"
            onClick={handleDownloadProductReport}
            disabled={!productReportQuery.data || !selectedProduct}
          >
            Download Excel
          </button>
        </div>
        <select className="input" value={productId} onChange={(e) => setProductId(e.target.value)}>
          <option value="">Select product</option>
          {productsQuery.data?.map((product) => (
            <option key={product.id} value={product.id}>
              {product.name}
            </option>
          ))}
        </select>
        <Status loading={productReportQuery.isLoading} error={productReportQuery.error?.message} />
        {productReportQuery.data ? (
          <div className="text-sm text-slate-700">
            Incoming: <span className="number-strong">{formatNumber(productReportQuery.data.incomingBaseQty)}</span> | Sold:{' '}
            <span className="number-strong">{formatNumber(productReportQuery.data.soldBaseQty)}</span> | In Stock:{' '}
            <span className="number-strong">{formatNumber(productReportQuery.data.inStockBaseQty)}</span>
          </div>
        ) : null}
      </div>

      <div className="card">
        <div className="mb-2 text-sm font-medium">Stock Report</div>
        <Status loading={stockQuery.isLoading} error={stockQuery.error?.message} />
        {stockQuery.data ? (
          <table className="table w-full">
            <thead className="border-b">
              <tr>
                <th>Product</th>
                <th>Incoming</th>
                <th>Sold</th>
                <th>In Stock</th>
              </tr>
            </thead>
            <tbody>
              {stockQuery.data.map((item) => (
                <tr key={item.productId}>
                  <td>{item.productName}</td>
                  <td className="number-strong">{formatNumber(item.incomingBaseQty)}</td>
                  <td className="number-strong">{formatNumber(item.soldBaseQty)}</td>
                  <td className="number-strong">{formatNumber(item.inStockBaseQty)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : null}
      </div>
    </div>
  );
}
