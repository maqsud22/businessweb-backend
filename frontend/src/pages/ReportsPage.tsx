import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../api/client';
import { Product, ProductReport, SalesSummary, StockReportItem } from '../api/types';
import PageHeader from '../components/PageHeader';
import Status from '../components/Status';

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
              Total: {dailyQuery.data.totalSales} | Cash: {dailyQuery.data.cashSales} | Debt: {dailyQuery.data.debtSales}
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
              Total: {monthlyQuery.data.totalSales} | Cash: {monthlyQuery.data.cashSales} | Debt: {monthlyQuery.data.debtSales}
            </div>
          ) : null}
        </div>
      </div>

      <div className="card space-y-3">
        <div className="text-sm font-medium">Product Report</div>
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
            Incoming: {productReportQuery.data.incomingBaseQty} | Sold: {productReportQuery.data.soldBaseQty} | In Stock:{' '}
            {productReportQuery.data.inStockBaseQty}
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
                  <td>{item.incomingBaseQty}</td>
                  <td>{item.soldBaseQty}</td>
                  <td>{item.inStockBaseQty}</td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : null}
      </div>
    </div>
  );
}
