# BusinessWeb Admin Frontend

Production-ready admin UI for BusinessWeb backend.

## ✅ Setup

```bash
cd frontend
npm install
```

## ✅ Environment

Create `.env` with:

```
VITE_API_BASE_URL=http://localhost:5091
```

(see `.env.example`)

## ✅ Development

```bash
npm run dev
```

## ✅ Build

```bash
npm run build
```

## ✅ Features

- JWT login at `/login`
- Protected admin routes
- CRUD for Stores, Products, Product Packages, Stock Ins
- Sales list + create + detail
- Debts list + detail + pay
- Reports: Daily, Monthly, Product, Stock

## ✅ API

Backend base URL defaults to `http://localhost:5091`. You can change it with `VITE_API_BASE_URL`.
