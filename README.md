# 📝 Blog Social Feed System

A RESTful API for a social blogging platform built with **ASP.NET Core 9**, following **Clean Architecture** and **Domain-Driven Design**. Supports users, posts, comments, and reactions with rich domain models, optimistic concurrency, and soft delete.

---

## 📋 Table of Contents

- [Features](#features)
- [Technical Highlights](#technical-highlights)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Future Improvements](#future-improvements)
- [Learning Goals](#learning-goals)
- [Author](#author)

---

## Features

- ✅ User management with paginated listing and search
- ✅ Post management with pagination and per-user filtering
- ✅ Comments on posts
- ✅ Reactions (Like / Love / Clap / Smile) on posts and comments, with one reaction per user enforced
- ✅ Soft delete across entities instead of permanent deletion
- ✅ Optimistic concurrency control on Users, Posts, and Comments
- ✅ Global exception-handling middleware
- ✅ Automatic database migration and seeding on startup
- ✅ Swagger / OpenAPI documentation

---

## Technical Highlights

- **Rich domain models & value objects** — entities encapsulate their own state (e.g. `Delete()` / `Restore()` on `BaseEntity`) instead of exposing public setters, and `Email` is modeled as a value object (`Email.Create(...)`) that normalizes and validates format at construction time rather than relying on ad-hoc string checks.
- **Optimistic concurrency** — `User`, `Post`, and `Comment` are configured with an EF Core `RowVersion` column, so a concurrent update on a stale copy of the same row fails instead of silently overwriting another user's change.
- **Reaction upsert instead of duplicates** — `POST /api/reactions` checks for an existing reaction by the same user on the same post/comment; if one exists, it updates the `Kind` in place instead of inserting a second row, keeping one reaction per user enforced at the service level.
- **Soft delete** — `IsDeleted` lives on `BaseEntity` alongside `Delete()`/`Restore()` domain methods, so records are hidden rather than physically removed.
- **Server-side pagination** — `GET /api/users` and `GET /api/posts` accept `pageNumber`/`pageSize` so listing endpoints don't load the full table per request.
- **Startup pipeline** — on boot, the API applies pending EF Core migrations and seeds initial data automatically (`Database.MigrateAsync()` + seeder), so a fresh clone is immediately usable.

---

## Architecture

The project follows **Clean Architecture** across four layers:

```
┌─────────────────────────────────────┐
│           Blog.Api (Presentation)   │  ← Controllers, Middleware
├─────────────────────────────────────┤
│        Blog.Application (Business)  │  ← Services, DTOs, Interfaces
├─────────────────────────────────────┤
│          Blog.Domain (Core)         │  ← Entities, Value Objects, Enums
├─────────────────────────────────────┤
│      Blog.Infrastructure (Data)     │  ← EF Core, Repositories, Migrations, Seeding
└─────────────────────────────────────┘
```

### Blog.Api
- Controllers (Users, Posts, Comments, Reactions)
- Global Exception Middleware
- Swagger configuration

### Blog.Application
- Services (`UserService`, `PostService`, `CommentService`, `ReactionService`)
- DTOs (request/response)
- Repository & Unit of Work interfaces

### Blog.Domain
- Entities (`User`, `Post`, `Comment`, `Reaction`) inheriting from `BaseEntity`
- Value Objects (`Email`)
- Enums (`ReactionKind`)

### Blog.Infrastructure
- `BlogDbContext` and EF Core configurations
- Repositories
- Migrations
- Seed data

---

## Tech Stack

| Technology | Notes |
|---|---|
| .NET | 9.0 |
| ASP.NET Core Web API | 9.0 |
| Entity Framework Core | 9.0 |
| SQL Server | via EF Core |
| Swashbuckle (Swagger) | API documentation |

---

## Project Structure

```
Blog_With_API/
│
├── Blog.Api/
│   ├── Controllers/
│   │   ├── UsersController.cs
│   │   ├── PostsController.cs
│   │   ├── CommentsController.cs
│   │   └── ReactionsController.cs
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Extensions/
│   └── Program.cs
│
├── Blog.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
│       ├── UserService.cs
│       ├── PostService.cs
│       ├── CommentService.cs
│       └── ReactionService.cs
│
├── Blog.Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   │   └── Email.cs
│   └── Enums/
│       └── ReactionKind.cs
│
└── Blog.Infrastructure/
    ├── Persistence/
    │   └── BlogDbContext.cs
    ├── Configurations/
    ├── Repositories/
    ├── Migrations/
    └── Seed Data/
```

---

## Design Patterns

### Repository Pattern
Each aggregate (`User`, `Post`, `Comment`, `Reaction`) has its own repository interface/implementation for data access, keeping EF Core details out of the service layer.

### Unit of Work Pattern
`IUnitOfWork` coordinates repositories and commits changes in a single `SaveChangesAsync` call per request.

### Value Objects
`Email` is a self-validating value object rather than a plain string property, so an invalid email can't exist on a `User` in the first place.

### Optimistic Concurrency
EF Core's `RowVersion` (configured via `IsRowVersion()` in the entity configurations) protects `User`, `Post`, and `Comment` from lost updates under concurrent writes.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

**1. Clone the repository**
```bash
git clone https://github.com/mhmdibz/BlogSocialFeed-API.git
cd BlogSocialFeed-API
```

**2. Configure the connection string**

Open `Blog.Api/appsettings.json` and update:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**3. Run the application**

Migrations and seeding run automatically on startup — no separate `dotnet ef database update` step is required:
```bash
dotnet run --project Blog.Api
```

**4. Open Swagger UI**
```
https://localhost:<port>/
```
(Swagger UI is served at the app root in development.)

---

## API Endpoints

22 endpoints across four resources:

### Users
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/users?pageNumber=&pageSize=` | Get users with pagination |
| GET | `/api/users/{id}` | Get user by ID |
| GET | `/api/users/search?term=` | Search users |
| GET | `/api/users/count` | Get total user count |
| POST | `/api/users` | Create a new user |
| PUT | `/api/users/{id}` | Update a user |
| DELETE | `/api/users/{id}` | Soft-delete a user |

### Posts
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/posts?pageNumber=&pageSize=` | Get posts with pagination |
| GET | `/api/posts/{id}` | Get post by ID |
| GET | `/api/posts/user/{userId}` | Get posts by a specific user |
| POST | `/api/posts` | Create a new post |
| PUT | `/api/posts/{id}` | Update a post |
| DELETE | `/api/posts/{id}` | Soft-delete a post |

### Comments
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/comments/{id}` | Get comment by ID |
| GET | `/api/comments/post/{postId}` | Get comments for a post |
| POST | `/api/comments` | Create a new comment |
| PUT | `/api/comments/{id}` | Update a comment |
| DELETE | `/api/comments/{id}` | Soft-delete a comment |

### Reactions
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/reactions/post/{postId}` | Get reactions for a post |
| GET | `/api/reactions/post/{postId}/counts` | Get reaction counts by kind for a post |
| POST | `/api/reactions` | React to a post/comment (updates the existing reaction instead of duplicating it) |
| DELETE | `/api/reactions/{id}` | Remove a reaction |

---

## Future Improvements

- [ ] JWT Authentication & Authorization
- [ ] Role-based access control
- [ ] Unit & integration testing
- [ ] Docker support
- [ ] CI/CD pipeline
- [ ] Notifications system
- [ ] Rate limiting

---

## Learning Goals

This project was built to practice:

- Clean Architecture principles
- Domain-Driven Design (rich models, value objects)
- Optimistic concurrency handling in EF Core
- Repository & Unit of Work patterns
- Building idempotent-style upsert logic (reactions)

---

## Author

**Mohamed Ibrahim Zaki**
Backend Engineer (ASP.NET Core) & Computer Science Student
[GitHub](https://github.com/mhmdibz) · [LinkedIn](https://www.linkedin.com/in/mohamed-ibrahim-dev-eg/)

---

## License

This project is for educational and portfolio purposes.
