# LuyPOS Clean Architecture

LuyPOS is designed as a production-ready POS backend using ASP.NET Core Web API on .NET 10, Entity Framework Core, PostgreSQL, JWT authentication, Docker, repositories, services, dependency injection, and RESTful APIs.

## Dependency Flow

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

Rules:

- API controllers call Application services only.
- Application services contain business logic and orchestration.
- Domain contains entities, enums, constants, domain events, and repository interfaces.
- Infrastructure implements Domain repository interfaces.
- Infrastructure owns EF Core, PostgreSQL, JWT token generation, external services, and migrations.
- Controllers never access `DbContext`.
- Controllers do not contain business logic.
- All I/O APIs are async.

## Project References

```text
LuyPOS.Domain
    no project references

LuyPOS.Application
    -> LuyPOS.Domain

LuyPOS.Infrastructure
    -> LuyPOS.Application
    -> LuyPOS.Domain

LuyPOS.Api
    -> LuyPOS.Application
    -> LuyPOS.Infrastructure
```

## Complete Folder Structure

```text
backend/
|-- LuyPOS.slnx
|-- src/
|   |-- LuyPOS.Api/
|   |   |-- Controllers/
|   |   |   |-- ProductsController.cs
|   |   |-- Filters/
|   |   |-- Middleware/
|   |   |   |-- ExceptionHandlingMiddleware.cs
|   |   |-- Extensions/
|   |   |-- Program.cs
|   |   |-- appsettings.json
|   |   `-- appsettings.Development.json
|   |-- LuyPOS.Application/
|   |   |-- Common/
|   |   |   `-- Exceptions/
|   |   |-- DTOs/
|   |   |   `-- Products/
|   |   |-- Features/
|   |   |   `-- Products/
|   |   |-- Interfaces/
|   |   |   `-- Services/
|   |   |-- Mappings/
|   |   |-- Validators/
|   |   `-- DependencyInjection.cs
|   |-- LuyPOS.Domain/
|   |   |-- Common/
|   |   |-- Constants/
|   |   |-- DomainEvents/
|   |   |-- Entities/
|   |   |-- Enums/
|   |   |-- Interfaces/
|   |   |   `-- Repositories/
|   |   `-- ValueObjects/
|   `-- LuyPOS.Infrastructure/
|       |-- Persistence/
|       |   |-- Configurations/
|       |   |-- Migrations/
|       |   `-- LuyPosDbContext.cs
|       |-- Repositories/
|       |-- Services/
|       |   |-- Jwt/
|       |   `-- External/
|       `-- DependencyInjection.cs
`-- tests/
    `-- LuyPOS.Api.Tests/
```

## Naming Conventions

- Projects: `LuyPOS.{Layer}`.
- Entities: singular nouns, for example `Product`, `Sale`, `Customer`.
- Tables: snake_case plural names, for example `products`, `sales`.
- DTOs: `{Action}{Entity}Request`, `{Entity}Response`.
- Services: `IProductService`, `ProductService`.
- Repositories: `IProductRepository`, `ProductRepository`.
- Controllers: plural resource names, for example `ProductsController`.
- API routes: lowercase plural resource routes, for example `api/products`.
- Async methods: suffix with `Async`.
- Exceptions: `{Reason}Exception`, for example `NotFoundException`.

## Product Module Example

```text
Product
|-- Entity
|   `-- Domain/Entities/Product.cs
|-- DTOs
|   `-- Application/DTOs/Products/*
|-- Repository Interface
|   `-- Domain/Interfaces/Repositories/IProductRepository.cs
|-- Repository Implementation
|   `-- Infrastructure/Repositories/ProductRepository.cs
|-- Entity Configuration
|   `-- Infrastructure/Persistence/Configurations/ProductConfiguration.cs
|-- Service Interface
|   `-- Application/Interfaces/Services/IProductService.cs
|-- Service Implementation
|   `-- Application/Features/Products/ProductService.cs
`-- Controller
    `-- Api/Controllers/ProductsController.cs
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

Responsibilities:

- `ProductsController` handles HTTP only: routing, status codes, and service calls.
- `ProductService` validates requests, enforces SKU uniqueness, maps DTOs, and decides soft-delete behavior.
- `IProductRepository` defines persistence operations in Domain.
- `ProductRepository` uses EF Core in Infrastructure.
- `ProductConfiguration` contains PostgreSQL table/index/column mapping.

## JWT Authentication Design

Place JWT abstractions in Application or Domain only when they represent business contracts:

```text
Application/Interfaces/Services/IAuthService.cs
Application/DTOs/Auth/LoginRequest.cs
Application/DTOs/Auth/AuthResponse.cs
Infrastructure/Services/Jwt/JwtTokenService.cs
Api/Controllers/AuthController.cs
```

Recommended JWT practices:

- Store password hashes only, never plain text.
- Keep JWT signing keys in environment variables or Docker secrets.
- Use short-lived access tokens and persisted refresh tokens.
- Add role and permission claims from the authorization model.
- Use `[Authorize]` at controller/action level.
- Keep token creation in Infrastructure; keep login rules in Application.

## Docker Design

Use Docker Compose for local development:

```text
api
postgres
```

Production recommendations:

- Run PostgreSQL in managed infrastructure or a hardened database container.
- Use environment variables for connection strings and JWT secrets.
- Add health checks for API and database.
- Build the API image with a multi-stage Dockerfile.
- Do not run migrations automatically on every API startup in production; run them as a release step.

## Best Practices

- Keep entities persistence-agnostic where possible.
- Keep business rules in services or domain methods, not controllers.
- Prefer explicit DTOs over exposing entities.
- Use EF configurations per entity for large modules.
- Use soft deletes for operational records that need history.
- Use database transactions for sales, payments, and inventory movements.
- Add optimistic concurrency tokens for high-contention inventory rows.
- Add pagination and filtering for list endpoints.
- Add structured logging and correlation IDs.
- Add API versioning before exposing public integrations.
- Add integration tests for repositories and application service flows.
- Use Problem Details for API errors.

## Scalability Recommendations

Inventory:

- Model stock as immutable inventory movements, not only a mutable quantity.
- Use `inventory_items`, `inventory_movements`, `stock_adjustments`, and `stock_transfers`.
- Add branch-aware stock tables for multi-location operations.
- Use transactions when sales reduce stock.

Sales:

- Split sales into `sales`, `sale_items`, `payments`, and `sale_discounts`.
- Use a transaction boundary around sale creation, payment recording, and stock deduction.
- Keep sale status explicit: draft, completed, voided, refunded.

Customers:

- Keep customer profiles separate from sales.
- Add loyalty fields in a dedicated module instead of bloating the base customer table.
- Index phone and normalized name for fast lookup at checkout.

Suppliers:

- Model purchase orders separately from suppliers.
- Add supplier products for supplier-specific SKU, cost, and lead time.
- Track payable status separately from receiving status.

Users, Roles, Permissions:

- Use role permissions for defaults and user permissions for overrides.
- Keep permission slugs stable, for example `products.create`.
- Cache permission claims per token lifetime.

Branches:

- Add `branches` and make inventory, sales, users, registers, and invoices branch-aware.
- Consider row-level authorization rules for branch-restricted users.

Invoices:

- Store invoice numbers as generated immutable values.
- Use branch/date/sequence based numbering if required by accounting.
- Keep invoice snapshots so historical invoices are not affected by product name or tax changes.

Reporting:

- Do not run expensive reports against transactional tables without indexes.
- Add read models or materialized views for daily sales, inventory valuation, and cashier summaries.
- Generate reports asynchronously for large date ranges.
- Consider CQRS/read models once operational queries and reporting start competing.
