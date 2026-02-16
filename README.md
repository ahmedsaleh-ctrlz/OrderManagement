# 🏬 Order Management System API

A clean and structured ASP.NET Core Web API project built using **Clean Architecture principles**.

This project demonstrates:

- Layered architecture (Domain / Application / Infrastructure / API)
- Dedicated repositories per module (No generic repository abuse)
- Business-driven services
- Stock management per warehouse
- Paging support
- Soft delete implementation
- DTO-based response mapping

---

## 🏗 Architecture

The project follows Clean Architecture:

- **Domain** → Entities & Enums
- **Application** → DTOs, Interfaces, Services, Business Logic
- **Infrastructure** → EF Core, Repositories, Entity Configurations
- **API** → Controllers & Middleware

---

## 📦 Core Modules

### 👤 Users
- Create user
- Update user
- Soft delete
- Paging
- Password hashing (BCrypt)

---

### 📦 Products
- Create product
- Unique SKU validation
- Paging
- Soft delete
- Integrated with stock creation

---

### 🏬 Warehouses
- Create warehouse
- Update warehouse
- Soft delete
- Paging

---

### 📊 Inventory (ProductStock)
Handles product quantities per warehouse:

- Add stock
- Deduct stock
- Get product quantity
- Get all products by warehouse
- Unique constraint on (ProductId, WarehouseId)

---

## 🔄 Business Logic Highlights

- Product creation automatically creates initial stock
- Stock cannot go below zero
- Soft delete handled via EF Core Query Filters
- All paging executed at database level
- Dedicated repository per aggregate (no generic repository)

---

## 🛠 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- BCrypt.Net
- Clean Architecture
- LINQ
- Dependency Injection

---

## 📌 Design Decisions

- Removed Generic BaseRepository to avoid architectural rigidity
- Introduced dedicated repositories for clarity and scalability
- Separated Product from ProductStock to support multi-warehouse design
- Business logic placed in Services layer
- Controllers remain thin

---

## 🚀 Future Improvements

- JWT Authentication
- Role-based authorization
- Order lifecycle (Confirm, Cancel, Complete)
- Stock concurrency handling (RowVersion)
- Logging & Monitoring
- Unit Testing

---

## 📊 Project Status

Core modules completed.
Architecture stabilized.
Ready for security layer integration.

---

## 👨‍💻 Author

Built as a Clean Architecture learning and portfolio project.
