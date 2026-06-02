# LuyPOS

LuyPOS is a Point of Sale system built with ASP.NET Core Web API, Clean Architecture, Entity Framework Core, PostgreSQL, Docker, and a Next.js frontend.

The backend targets .NET 10 and is organized so HTTP, business logic, domain rules, and infrastructure concerns stay separate.

## Tech Stack

- Backend: ASP.NET Core Web API, C#, .NET 10
- Architecture: Clean Architecture
- Database: PostgreSQL
- ORM: Entity Framework Core with Npgsql
- Authentication target: JWT authentication
- Backend patterns: Repository Pattern, Service Layer Pattern, Dependency Injection
- Frontend: Next.js, React, TypeScript
- Frontend package manager: pnpm
- Local services: Docker Compose

## Repository Structure

```text
LuyPOS/
|-- backend/
|   |-- docs/
|   |   |-- clean-architecture.md
|   |   `-- core-system-schema.md
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

## Backend Architecture

```text
LuyPOS.Api
    -> LuyPOS.Application
        -> LuyPOS.Domain

LuyPOS.Infrastructure
    -> LuyPOS.Application
    -> LuyPOS.Domain

LuyPOS.Api
    -> LuyPOS.Infrastructure
```

Layer rules:

- Controllers call Application services only.
- Services contain business logic and use cases.
- Repository interfaces live in Domain.
- Repository implementations live in Infrastructure.
- DTOs live in Application.
- Entities live in Domain.
- DbContext lives in Infrastructure.
- Controllers never access DbContext directly.
- All I/O work should use async/await.

## Backend Layers

### LuyPOS.Api

Owns HTTP concerns only.

```text
backend/src/LuyPOS.Api/
|-- Controllers/
|-- Middleware/
|-- Filters/
|-- Extensions/
|-- Program.cs
|-- appsettings.json
`-- appsettings.Development.json
```

Use this layer for controllers, API middleware, filters, request pipeline setup, OpenAPI setup, authentication registration, and dependency injection composition.

### LuyPOS.Application

Owns use cases and business workflows.

```text
backend/src/LuyPOS.Application/
|-- Common/
|-- DTOs/
|-- Features/
|-- Interfaces/
|-- Mappings/
|-- Validators/
`-- DependencyInjection.cs
```

Use this layer for DTOs, service interfaces, service implementations, validators, mappings, use cases, and application exceptions.

### LuyPOS.Domain

Owns the core business model.

```text
backend/src/LuyPOS.Domain/
|-- Common/
|-- Constants/
|-- DomainEvents/
|-- Entities/
|-- Enums/
|-- Interfaces/
|   `-- Repositories/
`-- ValueObjects/
```

Use this layer for entities, enums, value objects, constants, domain events, repository interfaces, and common base classes.

### LuyPOS.Infrastructure

Owns external details.

```text
backend/src/LuyPOS.Infrastructure/
|-- Persistence/
|   |-- Configurations/
|   |-- Migrations/
|   `-- LuyPosDbContext.cs
|-- Repositories/
|-- Services/
`-- DependencyInjection.cs
```

Use this layer for EF Core, PostgreSQL mappings, repository implementations, JWT services, external service clients, persistence, and migrations.

## Product Module Example

The Product module is implemented as the reference module for future POS features.

```text
Product
|-- Entity
|   `-- backend/src/LuyPOS.Domain/Entities/Product.cs
|-- DTOs
|   `-- backend/src/LuyPOS.Application/DTOs/Products/
|-- Repository Interface
|   `-- backend/src/LuyPOS.Domain/Interfaces/Repositories/IProductRepository.cs
|-- Repository Implementation
|   `-- backend/src/LuyPOS.Infrastructure/Repositories/ProductRepository.cs
|-- EF Configuration
|   `-- backend/src/LuyPOS.Infrastructure/Persistence/Configurations/ProductConfiguration.cs
|-- Service Interface
|   `-- backend/src/LuyPOS.Application/Interfaces/Services/IProductService.cs
|-- Service Implementation
|   `-- backend/src/LuyPOS.Application/Features/Products/ProductService.cs
`-- Controller
    `-- backend/src/LuyPOS.Api/Controllers/ProductsController.cs
```

Request flow:

```text
HTTP request
-> ProductsController
-> IProductService
-> IProductRepository
-> LuyPosDbContext
-> PostgreSQL
```

Current Product API routes:

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

## Core System Schema

The backend also includes a mapped core-system schema for users, roles, permissions, menus, languages, sessions, refresh tokens, audit logs, login histories, OTP, and password reset flows.

Read the schema notes here:

```text
backend/docs/core-system-schema.md
```

## Architecture Docs

Read the full Clean Architecture design here:

```text
backend/docs/clean-architecture.md
```

It includes:

- Complete folder structure
- Project references
- Dependency flow diagram
- Naming conventions
- Best practices
- Product module explanation
- Scaling recommendations for Inventory, Sales, Customer, Supplier, User, Role, Permission, Branch, Invoice, and Reporting modules

## Local Requirements

Install:

- Docker
- .NET 10 SDK
- Node.js
- pnpm

Check versions:

```bash
dotnet --version
node --version
pnpm --version
docker --version
```

On this machine, the .NET SDK may be available at:

```bash
/home/wintech/.dotnet/dotnet --version
```

## Database

PostgreSQL runs through Docker Compose.

Default local settings:

```text
Host: localhost
Port: 5432
Database: luypos
Username: postgres
Password: postgres
```

Start PostgreSQL:

```bash
docker compose up -d
```

If port `5432` is already used by a local PostgreSQL service, either stop that service or change the host port in `docker-compose.yml`.

## Run Backend

From the backend folder:

```bash
cd backend
dotnet restore
dotnet build
dotnet run --project src/LuyPOS.Api/LuyPOS.Api.csproj --launch-profile http
```

If `dotnet` is not on PATH:

```bash
cd backend
/home/wintech/.dotnet/dotnet restore LuyPOS.slnx
/home/wintech/.dotnet/dotnet build LuyPOS.slnx
/home/wintech/.dotnet/dotnet run --project src/LuyPOS.Api/LuyPOS.Api.csproj --launch-profile http
```

Default API URL:

```text
http://localhost:5178
```

OpenAPI JSON:

```text
http://localhost:5178/openapi/v1.json
```

## Run Frontend

From the frontend folder:

```bash
cd frontend
pnpm install
pnpm dev
```

If port `3000` is already used:

```bash
pnpm exec next dev -p 3001
```

Default frontend URL:

```text
http://localhost:3000
```

Alternative local URL:

```text
http://localhost:3001
```

## Useful Commands

Backend:

```bash
cd backend
dotnet restore
dotnet build
dotnet test
dotnet run --project src/LuyPOS.Api/LuyPOS.Api.csproj --launch-profile http
```

Frontend:

```bash
cd frontend
pnpm install
pnpm dev
pnpm build
pnpm lint
```

Docker:

```bash
docker compose up -d
docker compose ps
docker compose logs postgres
docker compose down
```

## Development Rules

- Keep controllers thin.
- Put business logic in Application services.
- Put persistence contracts in Domain repository interfaces.
- Put EF Core implementations in Infrastructure repositories.
- Return DTOs, not entities, from API endpoints.
- Use async/await for database and external service work.
- Use dependency injection for services and repositories.
- Add EF configurations for entities instead of putting all mapping in DbContext.
- Use migrations for database schema changes.
- Add tests for business rules and API behavior.

## Developer Guide

Use this guide when adding or changing backend features.

### 1. Create A New Module

For a module named `Category`, create files in this order:

```text
backend/src/LuyPOS.Domain/Entities/Category.cs
backend/src/LuyPOS.Domain/Interfaces/Repositories/ICategoryRepository.cs
backend/src/LuyPOS.Application/DTOs/Categories/CreateCategoryRequest.cs
backend/src/LuyPOS.Application/DTOs/Categories/UpdateCategoryRequest.cs
backend/src/LuyPOS.Application/DTOs/Categories/CategoryResponse.cs
backend/src/LuyPOS.Application/Interfaces/Services/ICategoryService.cs
backend/src/LuyPOS.Application/Features/Categories/CategoryService.cs
backend/src/LuyPOS.Infrastructure/Persistence/Configurations/CategoryConfiguration.cs
backend/src/LuyPOS.Infrastructure/Repositories/CategoryRepository.cs
backend/src/LuyPOS.Api/Controllers/CategoriesController.cs
```

Then register dependencies:

```text
backend/src/LuyPOS.Application/DependencyInjection.cs
backend/src/LuyPOS.Infrastructure/DependencyInjection.cs
backend/src/LuyPOS.Infrastructure/Persistence/LuyPosDbContext.cs
```

### 2. Layer Rules

API layer:

- Controllers may inject Application service interfaces only.
- Controllers may return HTTP status codes and DTOs.
- Controllers must not inject repositories.
- Controllers must not inject `LuyPosDbContext`.
- Controllers must not contain validation or business rules.

Application layer:

- Services coordinate use cases.
- Services validate input.
- Services call Domain repository interfaces.
- Services map entities to DTOs.
- Services throw Application exceptions such as `ValidationException` and `NotFoundException`.
- Services must not use EF Core directly.

Domain layer:

- Entities contain business state.
- Repository interfaces belong here.
- Domain must not reference Application, Infrastructure, or API.
- Domain must not depend on EF Core attributes unless there is a strong reason.

Infrastructure layer:

- Repositories implement Domain repository interfaces.
- Repositories use `LuyPosDbContext`.
- EF configurations define table names, indexes, column sizes, precision, and relationships.
- External providers such as JWT, email, SMS, payment gateways, and file storage belong here.

### 3. API Rules

Use REST resource names:

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

Do:

- Use plural controller routes.
- Use route constraints such as `{id:long}`.
- Use `CancellationToken` in controller actions.
- Return `CreatedAtAction` for successful creates.
- Return `NoContent` for successful deletes.

Do not:

- Put SQL or EF queries in controllers.
- Return Domain entities directly.
- Use magic strings for permission names when constants can be used.
- Catch every exception in controllers; use middleware for common exceptions.

### 4. DTO Rules

Use explicit request and response DTOs:

```text
CreateProductRequest
UpdateProductRequest
ProductResponse
```

Rules:

- Do not reuse EF entities as request bodies.
- Do not expose password hashes, tokens, or internal audit fields in response DTOs.
- Keep request DTOs close to the endpoint use case.
- Keep response DTOs stable for frontend consumption.

### 5. Validation Rules

Validate before saving:

- Required fields
- Maximum lengths
- Numeric ranges
- Duplicate business keys, such as SKU or invoice number
- Status transitions, such as draft to completed
- Permission and branch access

For simple validation, use validators in:

```text
backend/src/LuyPOS.Application/Validators/
```

For complex rules, keep logic in the Application service or Domain methods.

### 6. Repository Rules

Repositories should:

- Hide EF Core query details from Application services.
- Use async methods.
- Filter soft-deleted records by default.
- Use `AsNoTracking()` for read-only queries.
- Keep write operations explicit.

Repositories should not:

- Return DTOs.
- Contain business workflows.
- Commit unrelated aggregates in one method unless it is clearly a unit-of-work operation.

### 7. Database Rules

Use PostgreSQL-friendly design:

- Use snake_case table and column names.
- Use indexes for lookup fields and foreign keys.
- Use partial unique indexes for soft-deleted business keys.
- Use `numeric(18,2)` style precision for money values.
- Use transactions for sales, payments, stock movement, and invoice creation.
- Use migrations for schema changes.

Migration workflow when `dotnet-ef` is installed:

```bash
cd backend
dotnet ef migrations add AddProducts \
  --project src/LuyPOS.Infrastructure \
  --startup-project src/LuyPOS.Api

dotnet ef database update \
  --project src/LuyPOS.Infrastructure \
  --startup-project src/LuyPOS.Api
```

### 8. Testing Rules

Add tests for:

- Application service business rules
- Validation behavior
- Repository queries that are easy to break
- API endpoint status codes
- Sales and inventory transaction flows

Recommended test layout:

```text
backend/tests/LuyPOS.Api.Tests/
|-- Unit/
|   `-- Products/
|-- Integration/
|   `-- Products/
`-- Fixtures/
```

Run tests:

```bash
cd backend
dotnet test
```

### 9. Security Rules

- Never store plain text passwords.
- Keep JWT secrets outside source code.
- Use short-lived access tokens.
- Persist refresh tokens securely.
- Use role and permission claims.
- Check branch-level access for branch-specific resources.
- Log sensitive actions through audit logs.
- Do not log passwords, tokens, or payment card data.

### 10. POS Business Rules

Sales:

- A completed sale must create sale items, payment records, invoice data, and stock movements in one transaction.
- A sale cannot complete if stock is unavailable unless the business explicitly allows negative stock.
- Refunds and voids should create reversing records rather than deleting completed sales.

Inventory:

- Keep immutable stock movement history.
- Track stock by branch.
- Separate stock adjustment, stock transfer, purchase receiving, sale deduction, and return movement types.

Invoices:

- Invoice numbers must be unique and immutable.
- Store invoice snapshots for product name, price, tax, discount, and customer details.
- Do not recalculate historical invoices from changed product data.

Permissions:

- Use permission slugs such as `products.view`, `products.create`, `sales.refund`.
- Enforce permissions in Application services or authorization policies.
- Keep frontend permission checks as UX only; backend remains the source of truth.

### 11. Before Committing

Run:

```bash
cd backend
dotnet restore
dotnet build
dotnet test

cd ../frontend
pnpm install
pnpm lint
pnpm build
```

Checklist:

- The new feature follows Clean Architecture dependencies.
- Controller calls only services.
- Service contains business logic.
- Repository hides EF Core.
- DTOs are used for all API requests and responses.
- Database changes have migrations.
- README or docs are updated if workflow or structure changed.

## Future POS Modules

Recommended modules:

- Inventory
- Sales
- Customer
- Supplier
- User
- Role
- Permission
- Branch
- Invoice
- Reporting

Each module should follow the same pattern as Product:

```text
Domain entity
Application DTOs
Domain repository interface
Infrastructure repository implementation
Application service interface
Application service implementation
API controller
EF configuration
Tests
```

## Pull Request Checklist

- Backend restores successfully.
- Backend builds successfully.
- Backend tests pass or no tests are available yet.
- Frontend installs successfully.
- Frontend build/lint pass when frontend files change.
- New database changes include EF migrations.
- New endpoints follow REST naming.
- New modules follow the Product module structure.
- README or docs are updated when architecture or setup changes.
