# LuyPOS

LuyPOS is a Point of Sale system with a small ASP.NET Core Web API backend and a Next.js frontend.

## Tech Stack

- Backend: ASP.NET Core Web API, C#, .NET 10
- Database: PostgreSQL
- ORM: Entity Framework Core with Npgsql
- Frontend: Next.js, React, TypeScript
- Frontend package manager: pnpm
- Local services: Docker Compose

## Repository Structure

```text
LuyPOS/
|-- backend/
|   |-- LuyPOS.slnx
|   |-- src/
|   |   `-- LuyPOS.Api/
|   |       |-- Controllers/
|   |       |-- Data/
|   |       |-- Dtos/
|   |       |-- Middleware/
|   |       |-- Models/
|   |       |   |-- Core/
|   |       |   `-- Product.cs
|   |       |-- Properties/
|   |       |-- Services/
|   |       |-- appsettings.Development.json
|   |       |-- appsettings.json
|   |       |-- LuyPOS.Api.csproj
|   |       |-- LuyPOS.Api.http
|   |       `-- Program.cs
|   `-- tests/
|       `-- LuyPOS.Api.Tests/
|-- frontend/
|   `-- src/app/
|-- docker-compose.yml
`-- README.md
```

## Backend

The backend is intentionally simple:

- Controllers handle HTTP routes.
- Services contain app logic.
- Data contains the EF Core DbContext.
- Models contain database entities.
- Models/Core contains the existing core system models such as users, roles, permissions, menus, sessions, OTP, password reset, audit logs, and user activity.
- Dtos contain request and response shapes.

Current Product API routes:

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

## Local Setup

Start PostgreSQL:

```bash
docker compose up -d
```

Build backend:

```bash
/home/wintech/.dotnet/dotnet build backend/LuyPOS.slnx
```

Run backend:

```bash
/home/wintech/.dotnet/dotnet run --project backend/src/LuyPOS.Api/LuyPOS.Api.csproj
```

Run frontend:

```bash
cd frontend
pnpm install
pnpm run dev
```
