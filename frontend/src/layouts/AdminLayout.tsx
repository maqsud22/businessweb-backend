import { NavLink, Outlet } from 'react-router-dom';

const navItems = [
  { to: '/', label: 'Dashboard' },
  { to: '/stores', label: 'Stores' },
  { to: '/products', label: 'Products' },
  { to: '/product-packages', label: 'Product Packages' },
  { to: '/stock-ins', label: 'Stock In' },
  { to: '/sales', label: 'Sales' },
  { to: '/debts', label: 'Debts' },
  { to: '/reports', label: 'Reports' }
];

export default function AdminLayout() {
  return (
    <div className="min-h-screen bg-slate-50">
      <div className="flex">
        <aside className="min-h-screen w-64 border-r border-slate-200 bg-white p-4">
          <div className="mb-6 text-lg font-semibold">BusinessWeb Admin</div>
          <nav className="space-y-2">
            {navItems.map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) =>
                  `block rounded-md px-3 py-2 text-sm ${
                    isActive ? 'bg-slate-900 text-white' : 'text-slate-700 hover:bg-slate-100'
                  }`
                }
              >
                {item.label}
              </NavLink>
            ))}
          </nav>
        </aside>
        <main className="flex-1">
          <header className="border-b border-slate-200 bg-white p-4">
            <div className="text-sm text-slate-500">Admin Panel</div>
          </header>
          <div className="p-6">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
}
