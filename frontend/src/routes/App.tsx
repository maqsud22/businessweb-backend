import { Routes, Route } from 'react-router-dom';
import AdminLayout from '../layouts/AdminLayout';
import ProtectedRoute from './ProtectedRoute';
import LoginPage from '../pages/LoginPage';
import DashboardPage from '../pages/DashboardPage';
import StoresPage from '../pages/StoresPage';
import ProductsPage from '../pages/ProductsPage';
import ProductPackagesPage from '../pages/ProductPackagesPage';
import StockInsPage from '../pages/StockInsPage';
import SalesPage from '../pages/SalesPage';
import SaleDetailPage from '../pages/SaleDetailPage';
import SalesCreatePage from '../pages/SalesCreatePage';
import DebtsPage from '../pages/DebtsPage';
import DebtDetailPage from '../pages/DebtDetailPage';
import ReportsPage from '../pages/ReportsPage';

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route
        path="/"
        element={
          <ProtectedRoute>
            <AdminLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<DashboardPage />} />
        <Route path="stores" element={<StoresPage />} />
        <Route path="products" element={<ProductsPage />} />
        <Route path="product-packages" element={<ProductPackagesPage />} />
        <Route path="stock-ins" element={<StockInsPage />} />
        <Route path="sales" element={<SalesPage />} />
        <Route path="sales/new" element={<SalesCreatePage />} />
        <Route path="sales/:id" element={<SaleDetailPage />} />
        <Route path="debts" element={<DebtsPage />} />
        <Route path="debts/:id" element={<DebtDetailPage />} />
        <Route path="reports" element={<ReportsPage />} />
      </Route>
    </Routes>
  );
}
