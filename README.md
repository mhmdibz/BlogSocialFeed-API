# 📝 Blog API

A production-ready RESTful API for a blogging platform built with **ASP.NET Core 9**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## 🏗️ Architecture

```
Blog/
├── Blog.Domain          → Entities, Value Objects, Enums (no dependencies)
├── Blog.Application     → Interfaces, DTOs, Services (depends on Domain only)
├── Blog.Infrastructure  → EF Core DbContext, Repositories, UnitOfWork
├── Blog.Api             → ASP.NET Core Web API, Controllers, Middleware
└── Blog.Console         → Database seeder (seed test data)
```

---

## ✨ Features

- **Users** — Create, update, delete, search users
- **Posts** — Full CRUD with pagination
- **Comments** — Comment on posts
- **Reactions** — Like / Love / Clap / Smile on posts and comments
- **Soft Delete** — Records are never permanently deleted
- **Pagination** — All list endpoints support `pageNumber` & `pageSize`
- **Global Error Handling** — Consistent JSON error responses
- **Swagger UI** — Interactive API docs at `/` in development

---

## 🔗 API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/users` | Get all users (paginated) |
| GET | `/api/users/{id}` | Get user by ID |
| GET | `/api/users/search?term=` | Search users by username |
| GET | `/api/users/count` | Get total users count |
| POST | `/api/users` | Create user |
| PUT | `/api/users/{id}` | Update user |
| DELETE | `/api/users/{id}` | Delete user (soft) |
| GET | `/api/posts` | Get all posts (paginated) |
| GET | `/api/posts/{id}` | Get post by ID |
| GET | `/api/posts/user/{userId}` | Get posts by user |
| POST | `/api/posts` | Create post |
| PUT | `/api/posts/{id}` | Update post |
| DELETE | `/api/posts/{id}` | Delete post (soft) |
| GET | `/api/comments/{id}` | Get comment by ID |
| GET | `/api/comments/post/{postId}` | Get comments for a post |
| POST | `/api/comments` | Add comment |
| PUT | `/api/comments/{id}` | Update comment |
| DELETE | `/api/comments/{id}` | Delete comment (soft) |
| GET | `/api/reactions/post/{postId}` | Get reactions for a post |
| GET | `/api/reactions/post/{postId}/counts` | Get reaction counts by type |
| POST | `/api/reactions` | Add/update reaction |
| DELETE | `/api/reactions/{id}` | Remove reaction |

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

### 1. Clone the repo
```bash
git clone https://github.com/your-username/Blog.git
cd Blog
```

### 2. Configure connection string

Edit `Blog.Api/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Run database migrations
```bash
cd Blog.Api
dotnet ef database update --project ../Blog.Infrastructure
```

### 4. (Optional) Seed test data
```bash
cd Blog.Console
dotnet run
```

### 5. Run the API
```bash
cd Blog.Api
dotnet run
```

Open your browser at `https://localhost:{port}` → Swagger UI loads automatically.

---

## 🧱 Key Design Decisions

| Pattern | Usage |
|---------|-------|
| **Clean Architecture** | Dependencies only point inward (Domain ← Application ← Infrastructure ← API) |
| **Repository Pattern** | Abstracts data access behind interfaces |
| **Unit of Work** | Single `SaveChangesAsync` per request |
| **Value Object** | `Email` validates format and prevents invalid state |
| **Rich Domain Model** | Encapsulated collections, domain methods on entities |
| **Soft Delete** | `IsDeleted` flag on `BaseEntity` |
| **Concurrency Token** | `RowVersion` on `BaseEntity` for optimistic concurrency |
| **CancellationToken** | Propagated through all async operations |
| **Global Exception Middleware** | Centralized error handling with proper HTTP status codes |

---

## 📦 Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 9 Web API |
| ORM | Entity Framework Core 9 |
| Database | SQL Server |
| Documentation | Swagger / Swashbuckle |
| Fake Data | Bogus (Console seeder) |

---

## 📄 License

MIT
