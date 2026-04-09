# 🏗 Order Management System API

A production-style ASP.NET Core Web API built using Clean Architecture principles.  
The system simulates a real-world e-commerce backend that manages the complete order lifecycle and warehouse operations.  
It handles authentication, product management, stock control, order processing, and secure multi-role access across different system actors.

---

## 💡 System Overview

This project represents a simplified e-commerce and warehouse management system, including:

- Full order lifecycle management (create → confirm → cancel → ship → complete)  
- Warehouse management system with stock tracking per warehouse  
- Multi-role system (SuperAdmin, WarehouseAdmin, Employee, Customer)  
- Secure authentication and advanced authorization (role-based + ownership-based)  
- Business rule enforcement inside the service layer  

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
- Redis (Caching)  
- Serilog + Seq (Logging & Monitoring)  

---

## 🧱 Architecture

The project follows Clean Architecture:

Domain  
Application  
Infrastructure  
API  

### Domain
- Core entities  
- Enums  
- Business rules  

### Application
- Services  
- Interfaces  
- DTOs  
- Business Logic  
- Validation  
- Ownership Authorization  

### Infrastructure
- EF Core  
- Database persistence  
- Redis caching  
- Cancellation Tokens

### API
- Controllers  
- Middleware  
- JWT configuration  
- Logging (Serilog)  

---

## ⚡ Performance Optimization

- Implemented Redis caching to reduce database load and improve response time  
- Used async/await for better performance and scalability  

---

## 📊 Logging & Monitoring

- Structured logging using Serilog  
- Centralized logging using Seq  
- Supports debugging, tracing, and monitoring  

---

## 🔐 Authentication & Authorization

- JWT Authentication with custom claims  
- Role-based authorization  
- Ownership-based access control enforced in service layer  
-  Refresh Tokens to improve user exceprince

Roles:
- SuperAdmin → full access  
- WarehouseAdmin → manages warehouse  
- Employee → manages orders  
- Customer → manages own orders  

---

## 📦 Core Modules

Users:
- Registration & Login  
- Role management  

Warehouses:
- Managed by admins  
- Linked to employees  

Products:
- SKU uniqueness  
- Soft delete  
- Concurrency control  

Stock:
- Per warehouse tracking  
- Add / Deduct logic  

Orders:
- Full lifecycle  
- Ownership enforced  
- Stock updates on confirm  

---

## 🛡 Security Features

- BCrypt password hashing  
- JWT validation  
- Role & ownership authorization  
- Global exception handling  
- Optimistic concurrency  
- Soft delete filters  

---

## 🗄 Database Design

- Unique indexes (Email, SKU)  
- Foreign keys  
- Query filters  
- Concurrency tokens  

---

## 🧪 Example Flow

1. SuperAdmin is seeded  
2. Creates WarehouseAdmin  
3. Admin creates Employees  
4. Customer creates order  
5. Employee processes order  
6. Ownership rules enforced  

---

## ⚙ Configuration

Environment Variable:
JWT_SECRET_KEY=YourStrongSecretKeyHere

appsettings.json:
"JwtSettings": {
  "Issuer": "OrderManagementAPI",
  "Audience": "OrderManagementClient"
}

---

## 📌 What Makes This Project Strong

- Clean Architecture  
- Real-world business logic  
- Ownership-based authorization  
- Performance optimization (Redis)  
- Production-level logging  
- Scalable design  

---

## 🔮 Future Improvements

- Unit & Integration Tests  
- Docker  
- CI/CD  

