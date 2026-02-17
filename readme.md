# 🏗 Order Management System API

A production-style **ASP.NET Core Web API** built using Clean Architecture principles.  
The system simulates a real-world warehouse & order lifecycle with authentication, role-based authorization, and ownership-based access control.

---

## 🚀 Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt Password Hashing
- Clean Architecture
- Role-Based Authorization
- Ownership-Based Access Control
- Optimistic Concurrency (RowVersion)

---

## 🧱 Architecture

The project follows **Clean Architecture**:

```
Domain
Application
Infrastructure
API
```

### 🔹 Domain
- Core entities
- Enums
- Business rules
- No external dependencies

### 🔹 Application
- Services
- Interfaces (Repositories + Services)
- DTOs
- Business Logic
- Validation
- Ownership Authorization Logic

### 🔹 Infrastructure
- EF Core
- Repository implementations
- Entity configurations
- Database persistence

### 🔹 API
- Controllers
- JWT configuration
- Middleware
- Global exception handling

---

## 🔐 Authentication & Authorization

### JWT Authentication
- Custom JWT implementation
- Secret stored in Environment Variable
- Claims included:
  - UserId
  - Role
  - WarehouseId (when applicable)

### Roles Hierarchy

- **SuperAdmin**
  - Full system access
  - Creates Warehouse Admins

- **WarehouseAdmin**
  - Manages one warehouse
  - Can create Employees for their warehouse

- **WarehouseEmployee**
  - Manages orders inside assigned warehouse

- **Customer**
  - Creates orders
  - Can only access their own orders

---

## 🧠 Ownership-Based Authorization

Not just role-based access.

Business-level ownership checks ensure:

- Customers only access their own orders
- Warehouse admins/employees only access their warehouse data
- SuperAdmin bypasses ownership checks

Ownership validation is implemented inside the **Service Layer**, not Controllers.

---

## 📦 Core Modules

### 👤 Users
- Registration (Customer only)
- Login
- Role hierarchy
- Warehouse assignment

### 🏬 Warehouses
- Managed by SuperAdmin
- Linked to Admin/Employees

### 📦 Products
- SKU uniqueness
- Soft delete (IsDeleted)
- Concurrency control using RowVersion

### 📊 Stock Management
- ProductStock table
- Unique (ProductId + WarehouseId)
- Add / Deduct stock
- Quantity validation

### 🧾 Orders
- Full lifecycle:
  - Create
  - Confirm
  - Cancel
  - Ship
  - Complete
- Status stored as string enum
- Stock deducted on confirm
- Ownership enforced

---

## 🛡 Security Features

- BCrypt password hashing
- JWT token validation
- Role-based authorization
- Ownership-based business validation
- Global exception handling middleware
- Optimistic concurrency using RowVersion
- Soft delete query filters

---

## 🗄 Database Design Highlights

- Unique indexes (Email, SKU)
- Foreign key constraints
- Restrict delete behaviors
- Soft delete (IsDeleted)
- Query filters
- Optimistic concurrency tokens
- Warehouse-User mapping table

---

## 🧪 Example Flow

1. SuperAdmin is seeded automatically.
2. SuperAdmin creates WarehouseAdmin.
3. WarehouseAdmin creates Employees.
4. Customer registers and creates order.
5. WarehouseEmployee confirms and ships order.
6. Ownership rules prevent cross-warehouse access.

---

## ⚙ Configuration

### Environment Variable

```
JWT_SECRET_KEY=YourStrongSecretKeyHere
```

### appsettings.json

```
"JwtSettings": {
  "Issuer": "OrderManagementAPI",
  "Audience": "OrderManagementClient"
}
```

---

## 📌 What Makes This Project Strong

- Clean separation of concerns
- Removed Generic Repository anti-pattern
- Business rules enforced in Service layer
- Proper role hierarchy
- Ownership validation
- Optimistic concurrency handling
- Extensible and scalable structure

---

## 🔮 Future Improvements

- Refresh Tokens
- Rate Limiting
- Policy-based Authorization
- Audit logging
- Unit & Integration Tests
- Docker support
- CI/CD pipeline

---

## 📈 Project Level

Advanced Junior / Early Mid-Level Backend Project

Designed to demonstrate strong understanding of:

- Clean Architecture
- Secure API design
- Authorization design patterns
- Business rule enforcement
- Proper refactoring practices
- Scalable backend architecture

---

## 👨‍💻 Author

Built as a backend architecture exercise focused on real-world design patterns, scalability, and clean design principles.
