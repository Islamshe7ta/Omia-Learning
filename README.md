<div align="center">

<img src="https://img.shields.io/badge/-.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" />
<img src="https://img.shields.io/badge/-SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
<img src="https://img.shields.io/badge/-JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" />
<img src="https://img.shields.io/badge/-Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" />

#  Omia Learning

### Advanced Learning Management System — REST API

*A full-featured LMS backend built with clean architecture, designed for scalability and real-world production use.*

---

</div>

##  Overview

**Omia** is a robust Learning Management System API that enables organizations to manage courses, instructors, students, enrollments, and content delivery — all through a well-structured RESTful API.

Built with a **3-Layer Architecture** separating concerns cleanly across Presentation, Business Logic, and Data Access layers, making it highly maintainable and testable.

---

##  Key Features

-  **JWT Authentication & Role-Based Authorization** (Admin / Instructor / Student)
-  **Course Management** — Create, update, publish, and archive courses
-  **Instructor Profiles** — Manage instructor assignments per course
-  **Student Enrollment System** — Enroll, track progress, and manage subscriptions
-  **Content Management** — Lessons, sections, materials, and resources
-  **Progress Tracking** — Monitor student completion rates per course
-  **Reviews & Ratings** — Students can rate and review courses
-  **Email Notifications** — Triggered on enrollment, completion, etc.
-  **Advanced Filtering & Pagination** on all list endpoints
-  **Swagger UI** for full API documentation

---

## 🏗️ Architecture

The project follows a clean **3-Layer Architecture**:

```
Omia/
├── Omia.PL/          # Presentation Layer — Controllers, Middlewares, DTOs
│   ├── Controllers/
│   ├── Middlewares/
│   └── Program.cs
│
├── Omia.BLL/         # Business Logic Layer — Services, Interfaces, Mappings
│   ├── Services/
│   ├── Interfaces/
│   └── AutoMapper/
│
└── Omia.DAL/         # Data Access Layer — EF Core, Repositories, Migrations
    ├── Data/
    ├── Models/
    ├── Repositories/
    └── Migrations/
```

**Design Patterns Used:**
- Repository Pattern
- Unit of Work
- Generic Repository
- Dependency Injection (built-in .NET DI)
- DTO / AutoMapper Pattern

---

##  Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# 12 |
| Framework | ASP.NET Core 9 Web API |
| ORM | Entity Framework Core 9 |
| Database | Microsoft SQL Server |
| Auth | ASP.NET Core Identity + JWT Bearer |
| Mapping | AutoMapper |
| Documentation | Swagger / Swashbuckle |
| Validation | FluentValidation |

---

##  Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2022 / VS Code / Rider

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/Islamshe7ta/Omia-Learning.git
cd Omia-Learning

# 2. Restore packages
dotnet restore

# 3. Configure connection string in appsettings.json
# "ConnectionStrings": { "DefaultConnection": "your_sql_server_connection_string" }

# 4. Apply database migrations
dotnet ef database update --project Omia.DAL --startup-project Omia.PL

# 5. Run the API
dotnet run --project Omia.PL
```

### Access

Once running, open your browser at:
```
https://localhost:7000/swagger
```

---

##  API Endpoints Overview

| Module | Endpoints |
|--------|-----------|
| **Auth** | `POST /api/auth/register` · `POST /api/auth/login` · `POST /api/auth/refresh` |
| **Courses** | `GET /api/courses` · `POST /api/courses` · `PUT /api/courses/{id}` · `DELETE /api/courses/{id}` |
| **Enrollments** | `POST /api/enrollments` · `GET /api/enrollments/my` · `DELETE /api/enrollments/{id}` |
| **Lessons** | `GET /api/lessons/{courseId}` · `POST /api/lessons` · `PUT /api/lessons/{id}` |
| **Reviews** | `POST /api/reviews` · `GET /api/reviews/{courseId}` |
| **Progress** | `GET /api/progress/{courseId}` · `PUT /api/progress/complete/{lessonId}` |

> Full interactive documentation available via **Swagger UI** when running locally.

---

##  Database Schema (High-Level)

```
Users ──────────────── Enrollments ─── Courses
  │                                       │
  └── Instructors ──────────────────── Lessons
                                          │
                                      Materials
```

Key entities: `User`, `Course`, `Lesson`, `Enrollment`, `Progress`, `Review`, `Category`, `Instructor`

---

##  Authentication Flow

```
Client                          API
  │                              │
  ├── POST /auth/register ──────►│  Create user account
  ├── POST /auth/login ─────────►│  Returns Access Token + Refresh Token
  │                              │
  ├── GET /api/courses ─────────►│  Authorization: Bearer {token}
  │◄────────────────────────────┤  Returns protected data
  │                              │
  └── POST /auth/refresh ───────►│  Refresh expired token
```

---

## 👤 Author

**Islam Shehta**

[![GitHub](https://img.shields.io/badge/GitHub-Islamshe7ta-181717?style=flat-square&logo=github)](https://github.com/Islamshe7ta)

---

##  License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<div align="center">

*Built with ❤️ using ASP.NET Core*

</div>
