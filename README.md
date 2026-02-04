# BusinessWeb Backend (Clean Architecture, .NET 9)

Production-ready backend для BusinessWeb проекта. Quyida lokal ishga tushirish, migratsiya, Swagger authorize, va Render deploy bo‘yicha yo‘riqnoma berilgan.

## ✅ Local run

1) **Environment variables** (majburiy):

```bash
export ConnectionStrings__Default="Host=localhost;Port=5432;Database=businessweb;Username=postgres;Password=postgres"
export Jwt__Secret="PUT_LONG_RANDOM_SECRET_32+"
export Jwt__LifetimeMinutes="120"
```

2) **Restore & Run**

```bash
dotnet restore
dotnet run --project BusinessWeb.API
```

Swagger: `http://localhost:5000/swagger` (yoki konsolda ko‘rsatilgan URL)

> `appsettings.template.json` ichida placeholderlar berilgan. Prod uchun env varlardan foydalaning.

## ✅ EF Core migrations

```bash
dotnet ef database update --project BusinessWeb.Infrastructure --startup-project BusinessWeb.API
```

> `DatabaseSeeder` startup’da auto-migrate bajaradi, DB yo‘q bo‘lsa app crash qilmaydi.

## ✅ Swagger authorize (JWT)

1) `POST /api/Auth/register` yoki mavjud user bilan `POST /api/Auth/login`.
2) `token` ni oling.
3) Swagger UI’da **Authorize** tugmasini bosing.
4) `Bearer {token}` formatida kiriting.

## ✅ Test payloadlar

**POST /api/sales (Partial)**:

```json
{
  "storeId": "PUT_STORE_GUID",
  "paymentType": "Partial",
  "paidAmount": 50000,
  "lines": [
    { "productId": "PUT_PRODUCT_GUID_1", "productPackageId": "PUT_PACKAGE_GUID_1", "quantity": 2, "unitPrice": 12000 },
    { "productId": "PUT_PRODUCT_GUID_2", "productPackageId": "PUT_PACKAGE_GUID_2", "quantity": 5, "unitPrice": 8000 }
  ]
}
```

**POST /api/debts/pay**:

```json
{
  "debtId": "PUT_DEBT_GUID",
  "amount": 30000
}
```

## ✅ Reports

```
GET /api/reports/daily?date=2025-01-31
GET /api/reports/monthly?date=2025-01-01
GET /api/reports/product/{id}
GET /api/reports/stock
```

## ✅ Render deploy (Postgres + env vars)

1) Render’da **Postgres** yarating va connection string oling.
2) **Web Service** uchun env vars qo‘ying:
   - `ConnectionStrings__Default`
   - `Jwt__Secret`
   - `Jwt__LifetimeMinutes`
3) Deploy qiling. Startup’da auto-migrate ishlaydi.
