<div align="center">

<img src="https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=csharp&logoColor=white" />
<img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
<img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" />
<img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" />
<img src="https://img.shields.io/badge/AutoMapper-BE2025?style=for-the-badge" />

<br/><br/>

# Omia Learning

### Enterprise-Grade Learning Management System — REST API

*A production-ready LMS backend with 60+ API endpoints, real-time live sessions, quiz engine, assignment grading, and multi-role access control.*

<br/>

![API Endpoints](https://img.shields.io/badge/API%20Endpoints-60+-brightgreen?style=flat-square)
![Controllers](https://img.shields.io/badge/Controllers-15-blue?style=flat-square)
![Architecture](https://img.shields.io/badge/Architecture-3--Layer-orange?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

</div>

---

## Overview

**Omia** is a feature-rich Learning Management System API engineered for educational platforms. It supports the full lifecycle of online education — from course creation and student enrollment to live sessions, quiz attempts, assignment submissions, and graded feedback.

The system is built on a clean **3-Layer Architecture** with 15 dedicated API controllers, JWT-based multi-role authorization, and a comprehensive domain model covering every aspect of the e-learning experience.

---

## Features at a Glance

| Feature | Details |
|---------|---------|
|  **Auth & Identity** | JWT + Role-Based Access (Admin / Teacher / Student / Assistant) |
|  **Course Management** | Full CRUD, categories, notifications, details & brief views |
|  **Teacher & Assistant System** | Manage instructors, assign course assistants with role control |
|  **Student Management** | Enrollment, parent linking, per-course student lists |
|  **Assignment Engine** | Create assignments, accept submissions, grade per submission |
|  **Quiz Engine** | Create quizzes, start/end quiz sessions, track attempts per student |
|  **Live Sessions** | Schedule and manage real-time class sessions per course |
|  **Discussions & Chat** | Course-level discussions + direct user-to-user chat |
|  **Course Content** | Sections, lessons, comments, content count & newest content |
|  **Profile Management** | Granular profile updates (name, email, phone, location, password, image) |
|  **Categories** | Hierarchical course categorization with count endpoints |
|  **File Upload** | Centralized upload API for media and resources |

---

## Architecture

```
Omia/
├── Omia.PL/                    # Presentation Layer
│   ├── ApiControllers/         # 15 API Controllers
│   │   ├── AssignmentApiController.cs
│   │   ├── AssistantApiController.cs
│   │   ├── AuthApiController.cs
│   │   ├── CategoryApiController.cs
│   │   ├── CourseAssistantApiController.cs
│   │   ├── CourseContentApiController.cs
│   │   ├── CourseDiscussionApiController.cs
│   │   ├── CourseEnrollmentApiController.cs
│   │   ├── CoursesApiController.cs
│   │   ├── LiveSessionApiController.cs
│   │   ├── ProfileApiController.cs
│   │   ├── QuizApiController.cs
│   │   ├── StudentApiController.cs
│   │   ├── TeacherApiController.cs
│   │   └── UploadApiController.cs
│   └── Program.cs
│
├── Omia.BLL/                   # Business Logic Layer
│   ├── Services/
│   ├── Interfaces/
│   └── AutoMapper/
│
└── Omia.DAL/                   # Data Access Layer
    ├── Models/
    ├── Repositories/
    └── Migrations/
```

**Design Patterns:** Repository Pattern · Unit of Work · Generic Repository · AutoMapper DTOs · Dependency Injection

---

## Full API Reference

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/login` | Authenticate and receive JWT token |

---

### Courses
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/courses` | Create a new course |
| `GET` | `/api/courses` | Get all courses |
| `PUT` | `/api/courses/{courseId}` | Update course |
| `DELETE` | `/api/courses/{courseId}` | Delete course |
| `GET` | `/api/courses/{courseId}/details` | Get full course details |
| `GET` | `/api/courses/{courseId}/brief` | Get course summary |
| `GET` | `/api/courses/{courseId}/notifications` | Get course notifications |

---

### Categories
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/categories` | Create category |
| `PUT` | `/api/categories/{categoryId}` | Update category |
| `DELETE` | `/api/categories/{categoryId}` | Delete category |
| `GET` | `/api/categories/{categoryId}` | Get category by ID |
| `GET` | `/api/courses/{courseId}/categories` | Get course categories |
| `GET` | `/api/courses/{courseId}/categories/count` | Count course categories |

---

### Teachers
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/teachers` | Register teacher |
| `GET` | `/api/teachers` | Get all teachers |
| `PUT` | `/api/teachers/{teacherId}` | Update teacher |
| `DELETE` | `/api/teachers/{teacherId}` | Remove teacher |
| `GET` | `/api/teachers/{teacherId}` | Get teacher by ID |

---

### Assistants
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/assistants` | Add assistant |
| `GET` | `/api/assistants` | Get all assistants |
| `PUT` | `/api/assistants/{assistantId}` | Update assistant |
| `DELETE` | `/api/assistants/{assistantId}` | Remove assistant |
| `GET` | `/api/assistants/{assistantId}` | Get assistant by ID |

---

### Course Assistants
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/course-assistants` | Assign assistant to course |
| `DELETE` | `/api/course-assistants` | Remove assistant from course |
| `PUT` | `/api/course-assistants/roles` | Update assistant role in course |

---

### Students
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/students` | Register student |
| `GET` | `/api/students` | Get all students |
| `PUT` | `/api/students/{studentId}` | Update student |
| `DELETE` | `/api/students/{studentId}` | Remove student |
| `GET` | `/api/students/{studentId}` | Get student by ID |
| `PUT` | `/api/students/{studentId}/parent` | Link parent to student |
| `GET` | `/api/courses/{courseId}/students` | Get students per course |

---

### Enrollments
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/course-enrollments` | Enroll student in course |
| `DELETE` | `/api/course-enrollments` | Unenroll student |

---

### Course Content
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/course-content` | Add content item |
| `PUT` | `/api/course-content/{courseContentId}` | Update content |
| `DELETE` | `/api/course-content/{courseContentId}` | Delete content |
| `GET` | `/api/course-content/{contentId}` | Get content by ID |
| `POST` | `/api/course-content/comments` | Add comment to content |
| `GET` | `/api/courses/{courseId}/course-content/count` | Count content items |
| `GET` | `/api/courses/{courseId}/course-content/newest` | Get newest content |
| `GET` | `/api/courses/{courseId}/course-content` | Get all course content |
| `GET` | `/api/categories/{categoryId}/course-content` | Get content by category |

---

### Discussions & Chat
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/course-discussions` | Post a discussion |
| `GET` | `/api/courses/{courseId}/discussions` | Get course discussions |
| `GET` | `/api/chat` | Get all chats |
| `GET` | `/api/chat/{otherUserId}` | Get chat with specific user |

---

### Assignments
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/courses/{courseId}/assignments` | List course assignments |
| `POST` | `/api/assignments` | Create assignment |
| `PUT` | `/api/assignments/{assignmentId}` | Update assignment |
| `DELETE` | `/api/assignments/{assignmentId}` | Delete assignment |
| `POST` | `/api/assignments/submissions` | Submit assignment (student) |
| `GET` | `/api/assignments/{assignmentId}/submissions/student` | Get my submission |
| `PUT` | `/api/assignments/submissions/{submissionId}/grade` | Grade a submission |
| `GET` | `/api/assignments/{assignmentId}/submissions` | Get all submissions |

---

### Quizzes
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/quizzes` | Create quiz |
| `GET` | `/api/quizzes/{quizId}` | Get quiz by ID |
| `PUT` | `/api/quizzes/{quizId}` | Update quiz |
| `DELETE` | `/api/quizzes/{quizId}` | Delete quiz |
| `GET` | `/api/quizzes/course/{courseId}` | Get quizzes by course |
| `POST` | `/api/coursequiz/start` | Start quiz attempt |
| `POST` | `/api/coursequiz/end` | End quiz attempt |
| `GET` | `/api/quizzes/{quizId}/attempts/student` | Get my attempts |
| `GET` | `/api/quizzes/{quizId}/attempts` | Get all attempts (teacher view) |
| `GET` | `/api/quizzes/attempts/{attemptId}` | Get attempt details |

---

### Live Sessions
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/live-sessions` | Schedule live session |
| `PUT` | `/api/live-sessions/{liveSessionId}` | Update session |
| `DELETE` | `/api/live-sessions/{liveSessionId}` | Cancel session |
| `GET` | `/api/courses/{courseId}/live-sessions` | Get course sessions |

---

### Profile
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/profile` | Get my profile |
| `PUT` | `/api/profile` | Update full profile |
| `PUT` | `/api/profile/fullname` | Update full name |
| `PUT` | `/api/profile/username` | Update username |
| `PUT` | `/api/profile/email` | Update email |
| `PUT` | `/api/profile/phonenumber` | Update phone number |
| `PUT` | `/api/profile/location` | Update location |
| `PUT` | `/api/profile/password` | Change password |
| `PUT` | `/api/profile/image` | Update profile picture |

---

### Upload
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/UploadApi` | Upload files (images, videos, documents) |

---

## Tech Stack

| | Technology |
|--|-----------|
| **Language** | C# 12 |
| **Framework** | ASP.NET Core 9 Web API |
| **ORM** | Entity Framework Core 9 |
| **Database** | Microsoft SQL Server |
| **Auth** | ASP.NET Core Identity + JWT Bearer |
| **Mapping** | AutoMapper |
| **Documentation** | Swagger / Swashbuckle |

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2022 / VS Code / Rider

### Setup

```bash
# 1. Clone the repo
git clone https://github.com/Islamshe7ta/Omia-Learning.git
cd Omia-Learning

# 2. Restore dependencies
dotnet restore

# 3. Set your connection string in Omia.PL/appsettings.json
# "ConnectionStrings": { "DefaultConnection": "Server=.;Database=OmiaDB;Trusted_Connection=True;" }

# 4. Apply migrations
dotnet ef database update --project Omia.DAL --startup-project Omia.PL

# 5. Run the API
dotnet run --project Omia.PL
```

Then open **Swagger UI** at:
```
https://localhost:7000/swagger
```

---

## Authorization

All endpoints are protected with **JWT Bearer** authentication:

```http
Authorization: Bearer <your_token>
```

| Role | Access |
|------|--------|
| `Admin` | Full system access |
| `Teacher` | Manage own courses, content, assignments, quizzes |
| `Assistant` | Assist in assigned courses |
| `Student` | Enroll, submit, take quizzes, chat |

---

## Author

**Islam Shehta**

[![GitHub](https://img.shields.io/badge/GitHub-Islamshe7ta-181717?style=flat-square&logo=github)](https://github.com/Islamshe7ta)

---

## License

Licensed under the **MIT License** — see [LICENSE](LICENSE) for details.

---

<div align="center">

*Built with using ASP.NET Core 9*

</div>
