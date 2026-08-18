# Support Ticket Management API

A ASP.NET Core 8 project focused on secure backend engineering rather than CI/CD. It models a small support-ticket platform with API-key authentication, role-based authorization, rate limiting, validation, audit logging, health checks, and automated tests.

## Skills demonstrated

* ASP.NET Core REST API design
* Dependency injection and layered architecture
* Custom authentication with `AuthenticationHandler`
* Role-based authorization
* Built-in ASP.NET Core rate limiting
* Request validation and Problem Details responses
* Thread-safe repositories with `ConcurrentDictionary`
* Structured audit logging and correlation IDs
* Health checks
* Docker/containerization
* Unit testing with xUnit

## Architecture

```text
Client
  |
  v
Rate Limiter
  |
  v
Correlation Middleware
  |
  v
API-Key Authentication ---> Role Authorization
  |
  v
TicketsController
  |
  v
TicketService ----> Audit Logger
  |
  v
Thread-safe Ticket Repository
```

## Authentication

For local development the project supports two keys through configuration:

* Support agent: `dev-support-key`
* Admin: `dev-admin-key`

Send a key using the `X-API-Key` header. Never use development keys in production.

## Endpoints

|Method|Endpoint|Access|Purpose|
|-|-|-|-|
|GET|`/health`|Public|Health probe|
|GET|`/api/tickets`|Agent/Admin|List tickets|
|GET|`/api/tickets/{id}`|Agent/Admin|Get ticket|
|POST|`/api/tickets`|Agent/Admin|Create ticket|
|PATCH|`/api/tickets/{id}/status`|Agent/Admin|Update status|
|DELETE|`/api/tickets/{id}`|Admin|Delete ticket|

## Example request

```bash
curl -X POST http://localhost:8080/api/tickets \\
  -H "Content-Type: application/json" \\
  -H "X-API-Key: dev-support-key" \\
  -d '{"title":"Checkout failing","description":"Customers receive HTTP 500","severity":"High"}'
```

## Run locally

```bash
dotnet restore src/SupportTicket.Api/SupportTicket.Api.csproj
dotnet run --project src/SupportTicket.Api
```

## Run tests

```bash
dotnet test tests/SupportTicket.Tests/SupportTicket.Tests.csproj
```

## Docker

```bash
docker compose up --build
```

## Production improvements

This portfolio version intentionally uses an in-memory repository so the security and application architecture stay easy to inspect. A production version could add PostgreSQL/SQL Server, Entity Framework Core, distributed caching, OpenTelemetry, centralized secrets management, and external identity/OAuth.

