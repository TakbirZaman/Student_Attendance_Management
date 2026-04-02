# 📋 Attendance Management API

> A production-grade RESTful API built with **ASP.NET Core 8**, following **N-Tier Clean Architecture** with JWT authentication, EF Core ORM, and Swagger documentation.

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────┐
│                   Attendance.API                    │  ← Controllers, Middlewares, Program.cs
├─────────────────────────────────────────────────────┤
│              Attendance.Application                 │  ← Services, DTOs, Interfaces
├─────────────────────────────────────────────────────┤
│             Attendance.Infrastructure               │  ← Repositories, EF Core, DbContext
├─────────────────────────────────────────────────────┤
│               Attendance.Domain                     │  ← Entities, Domain Models
└─────────────────────────────────────────────────────┘
                          │
                    ┌─────▼──────┐
                    │  Database  │  ← SQL Server / SQLite
                    └────────────┘
```

Each layer depends only on the layer below it — no circular dependencies, no tight coupling.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🔐 **JWT Authentication** | Stateless token-based auth for all protected endpoints |
| 🧱 **N-Tier Architecture** | Clean separation across Domain, Infrastructure, Application, and API layers |
| 🔗 **Repository Pattern** | Abstracted data access — swap databases without touching business logic |
| ⚡ **LINQ Queries** | Efficient, readable querying with EF Core |
| 📄 **Swagger / OpenAPI** | Auto-generated interactive API documentation |
| 🔄 **Async / Await** | Fully asynchronous pipeline for high throughput |
| 🗂️ **DTO Mapping** | Clean request/response contracts — domain models never exposed directly |

---

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8
- **ORM**: Entity Framework Core
- **Database**: SQL Server (or SQLite for local dev)
- **Auth**: JWT Bearer Tokens
- **Docs**: Swashbuckle / Swagger UI
- **Language**: C# 12

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or update connection string for SQLite)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/attendance-api.git
cd attendance-api
```

### 2. Configure the database

Update `appsettings.json` in `Attendance.API`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=AttendanceDb;Trusted_Connection=True;"
}
```

### 3. Apply migrations

```bash
cd Attendance.API
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

### 5. Open Swagger UI

Navigate to: **`https://localhost:5001/swagger`**

---

## 📡 API Endpoints

### 🔓 Auth

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/auth/login` | Login and receive a JWT token |

### 👩‍🎓 Students _(requires JWT)_

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/students` | Get all students |
| `GET` | `/api/students/{id}` | Get student by ID |
| `POST` | `/api/students` | Create a new student |
| `DELETE` | `/api/students/{id}` | Delete a student |

### 📅 Attendance _(requires JWT)_

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/attendance` | Get all attendance records |
| `POST` | `/api/attendance` | Mark attendance |
| `GET` | `/api/attendance/student/{id}` | Get records by student |

---

## 📁 Project Structure

```
AttendanceSystem/
│
├── Attendance.API/
│   ├── Controllers/
│   │   ├── StudentsController.cs
│   │   └── AttendanceController.cs
│   └── Program.cs
│
├── Attendance.Application/
│   ├── Services/
│   │   ├── IStudentService.cs
│   │   └── StudentService.cs
│   └── DTOs/
│       ├── StudentDto.cs
│       └── CreateStudentDto.cs
│
├── Attendance.Infrastructure/
│   ├── Repositories/
│   │   ├── IStudentRepository.cs
│   │   └── StudentRepository.cs
│   └── Data/
│       └── AppDbContext.cs
│
└── Attendance.Domain/
    ├── Student.cs
    └── Attendance.cs





---

<p align="center">Built with ASP.NET Core 8 · Entity Framework Core · JWT Auth</p>
