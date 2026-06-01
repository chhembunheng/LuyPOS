# LuyPOS

Clean starter workspace for LuyPOS with an ASP.NET Core Web API backend, Next.js frontend, pnpm, and PostgreSQL.

## Structure

- `backend/src/LuyPOS.Api` - ASP.NET Core Web API entry point
- `backend/src/LuyPOS.Application` - application layer
- `backend/src/LuyPOS.Domain` - domain layer
- `backend/src/LuyPOS.Infrastructure` - infrastructure layer and PostgreSQL packages
- `backend/tests/LuyPOS.Api.Tests` - backend test project
- `frontend` - Next.js app using pnpm
- `docker-compose.yml` - local PostgreSQL service

## Commands

```powershell
docker compose up -d
cd backend
dotnet build
dotnet test
dotnet run --project .\src\LuyPOS.Api\LuyPOS.Api.csproj
```

```powershell
cd frontend
pnpm install
pnpm dev
```

