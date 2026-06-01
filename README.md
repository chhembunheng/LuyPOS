# LuyPOS

LuyPOS is a clean starter workspace for a POS system. The repository contains an ASP.NET Core Web API backend, a Next.js frontend, pnpm for frontend packages, and PostgreSQL for local development.

## Detected Project Type

- Backend: ASP.NET Core Web API, C#, .NET 10
- Backend architecture: layered API structure
- Database: PostgreSQL through Docker Compose
- Backend data packages: Entity Framework Core with Npgsql provider
- Backend tests: xUnit
- Frontend: Next.js, React, TypeScript
- Frontend package manager: pnpm

## Repository Structure

```text
LuyPOS/
|-- backend/
|   |-- LuyPOS.slnx
|   |-- src/
|   |   |-- LuyPOS.Api/
|   |   |-- LuyPOS.Application/
|   |   |-- LuyPOS.Domain/
|   |   `-- LuyPOS.Infrastructure/
|   `-- tests/
|       `-- LuyPOS.Api.Tests/
|-- frontend/
|   `-- src/app/
|-- docker-compose.yml
`-- README.md
```

## Backend API Structure

The backend uses separate C# projects so each layer has a clear job.

### `backend/src/LuyPOS.Api`

This is the Web API entry point. It owns HTTP concerns only.

Use it for:

- `Program.cs`
- API controllers
- middleware
- dependency injection extension methods
- request/response behavior
- OpenAPI setup

Current folders:

```text
LuyPOS.Api/
|-- Controllers/
|-- Extensions/
|-- Middleware/
|-- Properties/
|-- appsettings.json
|-- appsettings.Development.json
|-- LuyPOS.Api.csproj
`-- Program.cs
```

Example controller location:

```text
backend/src/LuyPOS.Api/Controllers/ProductsController.cs
```

Example controller shape:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace LuyPOS.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(new
        {
            status = "success",
            data = Array.Empty<object>()
        });
    }
}
```

### `backend/src/LuyPOS.Application`

This layer contains application/business use cases. It should not depend on ASP.NET Core controller logic.

Use it for:

- DTOs
- request/response models
- use cases
- application services
- interfaces/abstractions
- validation rules
- shared application behavior

Current folders:

```text
LuyPOS.Application/
|-- Abstractions/
|-- Common/
|-- DTOs/
`-- Features/
```

Example feature structure:

```text
backend/src/LuyPOS.Application/Features/Products/
|-- CreateProduct/
|-- GetProductById/
`-- ListProducts/
```

Example DTO location:

```text
backend/src/LuyPOS.Application/DTOs/ProductDto.cs
```

Example DTO:

```csharp
namespace LuyPOS.Application.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price
);
```

### `backend/src/LuyPOS.Domain`

This layer contains the core business model. It should stay independent from API and database details.

Use it for:

- entities
- enums
- value objects
- domain rules
- domain constants

Current folders:

```text
LuyPOS.Domain/
|-- Common/
|-- Entities/
|-- Enums/
`-- ValueObjects/
```

Example entity location:

```text
backend/src/LuyPOS.Domain/Entities/Product.cs
```

Example entity:

```csharp
namespace LuyPOS.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
}
```

### `backend/src/LuyPOS.Infrastructure`

This layer contains external details such as database access and third-party services.

Use it for:

- Entity Framework Core DbContext
- database configurations
- repositories
- infrastructure services
- PostgreSQL setup
- migrations when EF Core is configured

Current folders:

```text
LuyPOS.Infrastructure/
|-- Persistence/
`-- Services/
```

Example persistence structure:

```text
backend/src/LuyPOS.Infrastructure/Persistence/
|-- LuyPosDbContext.cs
`-- Configurations/
    `-- ProductConfiguration.cs
```

Example DbContext:

```csharp
using LuyPOS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuyPOS.Infrastructure.Persistence;

public sealed class LuyPosDbContext(DbContextOptions<LuyPosDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
```

### `backend/tests/LuyPOS.Api.Tests`

This project contains backend tests.

Use it for:

- API integration tests
- controller tests
- application service tests
- unit tests for business logic

Current folders:

```text
LuyPOS.Api.Tests/
|-- Integration/
`-- Unit/
```

Example test location:

```text
backend/tests/LuyPOS.Api.Tests/Integration/ProductsControllerTests.cs
```

## Backend Project References

The projects are connected like this:

```text
LuyPOS.Api
|-- references LuyPOS.Application
`-- references LuyPOS.Infrastructure

LuyPOS.Infrastructure
|-- references LuyPOS.Application
`-- references LuyPOS.Domain

LuyPOS.Application
`-- references LuyPOS.Domain
```

Simple rule:

- API calls Application.
- Application uses Domain.
- Infrastructure implements database and external services.
- Domain does not depend on other layers.

## Where To Put New Backend Code

When adding a new API feature, use this guide:

| Need | Put it here |
| --- | --- |
| HTTP endpoint | `backend/src/LuyPOS.Api/Controllers` |
| API middleware | `backend/src/LuyPOS.Api/Middleware` |
| Dependency injection setup | `backend/src/LuyPOS.Api/Extensions` |
| DTO/request/response model | `backend/src/LuyPOS.Application/DTOs` |
| Use case or feature logic | `backend/src/LuyPOS.Application/Features` |
| Service interface | `backend/src/LuyPOS.Application/Abstractions` |
| Entity | `backend/src/LuyPOS.Domain/Entities` |
| Enum | `backend/src/LuyPOS.Domain/Enums` |
| Value object | `backend/src/LuyPOS.Domain/ValueObjects` |
| DbContext/repository/configuration | `backend/src/LuyPOS.Infrastructure/Persistence` |
| External service implementation | `backend/src/LuyPOS.Infrastructure/Services` |
| Integration test | `backend/tests/LuyPOS.Api.Tests/Integration` |
| Unit test | `backend/tests/LuyPOS.Api.Tests/Unit` |

## Example: Adding A Products API

Recommended file layout:

```text
backend/src/LuyPOS.Api/Controllers/ProductsController.cs
backend/src/LuyPOS.Application/DTOs/ProductDto.cs
backend/src/LuyPOS.Application/Features/Products/ListProducts/ListProductsQuery.cs
backend/src/LuyPOS.Application/Features/Products/CreateProduct/CreateProductCommand.cs
backend/src/LuyPOS.Domain/Entities/Product.cs
backend/src/LuyPOS.Infrastructure/Persistence/LuyPosDbContext.cs
backend/src/LuyPOS.Infrastructure/Persistence/Configurations/ProductConfiguration.cs
backend/tests/LuyPOS.Api.Tests/Integration/ProductsControllerTests.cs
```

Expected API routes could look like:

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

## Frontend Structure

The frontend is a Next.js app using the App Router.

```text
frontend/
|-- public/
|-- src/
|   `-- app/
|       |-- globals.css
|       |-- layout.tsx
|       `-- page.tsx
|-- package.json
|-- pnpm-lock.yaml
`-- tsconfig.json
```

Use `frontend/src/app` for pages, layouts, and app-level styling. Add shared frontend folders when needed, for example:

```text
frontend/src/components/
frontend/src/lib/
frontend/src/services/
frontend/src/types/
```

## Local Requirements

Install these before running the project:

- Docker Desktop
- .NET 10 SDK
- Node.js
- pnpm

Check versions:

```powershell
dotnet --version
node --version
pnpm --version
docker --version
```

## Environment Configuration

PostgreSQL runs from `docker-compose.yml`.

Default local database values:

```text
Host: localhost
Port: 5432
Database: luypos
Username: postgres
Password: postgres
```

Frontend environment example:

```text
frontend/.env.example
```

Copy it when local frontend environment variables are needed:

```powershell
cd frontend
Copy-Item .env.example .env.local
```

## Run The Project

Start PostgreSQL:

```powershell
docker compose up -d
```

Build and test the backend:

```powershell
cd backend
dotnet restore
dotnet build
dotnet test
```

Run the backend API:

```powershell
cd backend
dotnet run --project .\src\LuyPOS.Api\LuyPOS.Api.csproj
```

Run the frontend:

```powershell
cd frontend
pnpm install
pnpm dev
```

## Useful Commands

Backend:

```powershell
cd backend
dotnet restore
dotnet build
dotnet test
dotnet run --project .\src\LuyPOS.Api\LuyPOS.Api.csproj
```

Frontend:

```powershell
cd frontend
pnpm install
pnpm dev
pnpm build
pnpm lint
```

Docker:

```powershell
docker compose up -d
docker compose ps
docker compose logs postgres
docker compose down
```

## API Development Rules For This Repository

- Keep controllers thin.
- Put business/use-case logic in `LuyPOS.Application`.
- Put entities and core business rules in `LuyPOS.Domain`.
- Put database and external service implementations in `LuyPOS.Infrastructure`.
- Validate incoming API requests before saving data.
- Return consistent JSON responses.
- Add tests for new API behavior.
- Run `dotnet test` before opening a pull request.

## Pull Request Checklist

Before sending code for review:

- Backend builds with `dotnet build`.
- Backend tests pass with `dotnet test`.
- Frontend builds with `pnpm build`.
- Frontend lint passes with `pnpm lint`.
- New API endpoints include tests.
- Database changes include EF Core migrations when EF Core is configured.
- README or docs are updated if structure or setup changes.
