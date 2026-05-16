# Blog Social Feed API

A production-ready RESTful API for a blogging platform built with **ASP.NET Core 9**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=flat&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=flat&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoftsqlserver&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-UI-85EA2D?style=flat&logo=swagger&logoColor=black)

---

## 📌 Overview

Blog Social Feed API allows users to create posts, comment, react with emotions (Like, Love, Clap, Smile), and follow each other — all exposed through a clean, well-structured REST API with Swagger documentation.

---

## 🏗️ Architecture

```
BlogSocialFeed-API/
├── Blog.Domain          → Entities, Value Objects, Enums       (no dependencies)
├── Blog.Application     → Interfaces, DTOs, Services           (depends on Domain only)
├── Blog.Infrastructure  → EF Core, Repositories, UnitOfWork    (depends on Application)
└── Blog.Api             → Controllers, Middleware, Program.cs  (depends on Infrastructure)
```

> Dependencies only flow **inward** — the Domain layer has zero external dependencies.

---

## ✨ Features

- **Users** — Create, update, soft-delete, and search users
- **Posts** — Full CRUD with pagination
- **Comments** — Add and manage comments on posts
- **Reactions** — Like / Love / Clap / Smile on posts and comments (upsert logic)
- **Soft Delete** — Records are never permanently removed from the database
- **Pagination** — All list endpoints support `pageNumber` & `pageSize`
- **Auto Migration & Seeding** — Database is created and seeded automatically on startup
- **Global Exception Handling** — Consistent JSON error responses across all endpoints
- **Swagger UI** — Interactive API documentation available in development

---

## 🔗 API Endpoints

### Users
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/users` | Get all users (paginated) |
| `GET` | `/api/users/{id}` | Get user by ID |
| `GET` | `/api/users/search?term=` | Search users by username |
| `GET` | `/api/users/count` | Get total users count |
| `POST` | `/api/users` | Create a new user |
| `PUT` | `/api/users/{id}` | Update user |
| `DELETE` | `/api/users/{id}` | Soft delete user |

### Posts
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/posts` | Get all posts (paginated) |
| `GET` | `/api/posts/{id}` | Get post by ID |
| `GET` | `/api/posts/user/{userId}` | Get posts by user |
| `POST` | `/api/posts` | Create a new post |
| `PUT` | `/api/posts/{id}` | Update post |
| `DELETE` | `/api/posts/{id}` | Soft delete post |

### Comments
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/comments/{id}` | Get comment by ID |
| `GET` | `/api/comments/post/{postId}` | Get all comments for a post |
| `POST` | `/api/comments` | Add a comment |
| `PUT` | `/api/comments/{id}` | Update comment |
| `DELETE` | `/api/comments/{id}` | Soft delete comment |

### Reactions
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/reactions/post/{postId}` | Get all reactions for a post |
| `GET` | `/api/reactions/post/{postId}/counts` | Get reaction counts grouped by type |
| `POST` | `/api/reactions` | Add or update a reaction |
| `DELETE` | `/api/reactions/{id}` | Remove a reaction |

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2022+ or VS Code

### 1. Clone the repository
```bash
git clone https://github.com/mhmdibz/BlogSocialFeed-API.git
cd BlogSocialFeed-API
```

### 2. Configure the connection string
Edit `Blog.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Run the API
```bash
cd Blog.Api
dotnet run
```

> On first run, the app automatically applies all pending migrations and seeds the database with fake data (50 users, 100 posts, 200 comments, and reactions).

### 4. Open Swagger UI
Navigate to `https://localhost:{port}` in your browser — Swagger UI loads automatically.

---

## 🧱 Key Design Decisions

| Pattern | Implementation |
|---------|---------------|
| **Clean Architecture** | 4 isolated layers with inward-only dependencies |
| **Domain-Driven Design** | Value Objects (`Email`), Rich Domain Model, encapsulated collections |
| **Repository Pattern** | Generic interfaces per entity in the Application layer |
| **Unit of Work** | Single `SaveChangesAsync` call per request |
| **Soft Delete** | `IsDeleted` flag on `BaseEntity` — no hard deletes |
| **Optimistic Concurrency** | `RowVersion` token on `BaseEntity` |
| **CancellationToken** | Propagated through all async operations |
| **Global Exception Middleware** | Maps exceptions to consistent HTTP status codes |
| **Auto Seeding** | Bogus library generates realistic fake data on startup |

---

## 📦 Tech Stack

| | Technology |
|--|-----------|
| **Framework** | ASP.NET Core 9 Web API |
| **ORM** | Entity Framework Core 9 |
| **Database** | SQL Server |
| **Documentation** | Swagger / Swashbuckle |
| **Fake Data** | Bogus |

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
