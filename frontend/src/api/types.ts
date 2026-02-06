export type PaymentType = 'Cash' | 'Debt' | 'Partial';

export interface Store {
  id: string;
  name: string;
  phone: string;
  isActive: boolean;
  createdAt?: string;
}

export interface Product {
  id: string;
  name: string;
  unit: number;
  defaultPrice: number;
  createdAt?: string;
}

export interface ProductPackage {
  id: string;
  name: string;
  multiplier: number;
  productId: string;
  productName?: string;
  createdAt?: string;
}

export interface StockIn {
  id: string;
  productId: string;
  productName?: string;
  productPackageId: string;
  productPackageName?: string;
  quantity: number;
  costPrice: number;
  date: string;
  createdAt?: string;
}

export interface SaleLine {
  id?: string;
  productId: string;
  productPackageId: string;
  quantity: number;
  unitPrice: number;
  lineTotal?: number;
}

export interface Sale {
  id: string;
  storeId: string;
  paymentType: PaymentType;
  totalAmount: number;
  createdAt: string;
  lines?: SaleLine[];
}

export interface Debt {
  id: string;
  saleId: string;
  total: number;
  paid: number;
  isClosed: boolean;
  createdAt: string;
  payments?: { id: string; amount: number; date: string }[];
}

export interface SalesSummary {
  from: string;
  to: string;
  totalSales: number;
  cashSales: number;
  debtSales: number;
  partialSales: number;
  debtPayments: number;
  saleCount: number;
}

export interface StockReportItem {
  productId: string;
  productName: string;
  incomingBaseQty: number;
  soldBaseQty: number;
  inStockBaseQty: number;
}

export interface ProductReport {
  productId: string;
  productName: string;
  incomingBaseQty: number;
  soldBaseQty: number;
  inStockBaseQty: number;
}
