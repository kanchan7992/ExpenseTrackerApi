# 💰 Expense Tracker API

A RESTful Expense Tracker API built using ASP.NET Core that allows users to manage expenses with secure authentication and advanced querying features.

---

## 🚀 Features

- 🔐 JWT-based authentication (Register/Login)
- 📂 Category management (user-specific)
- 💸 Expense tracking (CRUD operations)
- 🔍 Filtering (category, amount, date)
- 📄 Pagination & sorting
- 📊 Monthly summary (aggregation using GROUP BY & SUM)
- ✅ Input validation & error handling

---

## 🛠 Tech Stack

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQLite
- JWT Authentication

---

## 📌 API Endpoints

### Auth
- POST `/api/auth/register`
- POST `/api/auth/login`

### Categories
- POST `/api/categories`
- GET `/api/categories`

### Expenses
- POST `/api/expenses`
- GET `/api/expenses` (with filtering, pagination, sorting)

### Summary
- GET `/api/expenses/summary?month=4&year=2026`

---

## ⚡ How to Run

```bash
git clone https://github.com/kanchan7992/ExpenseTrackerApi.git
cd ExpenseTrackerApi
dotnet restore
dotnet run
