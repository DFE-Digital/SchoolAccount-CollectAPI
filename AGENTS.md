# AGENTS.md

## Stack
ASP.NET Core 10, C# 14, Entity Framework Core.

## Language & Style
- Favor explicit typing — only use `var` when the type is evident from the right-hand side
- Make types `internal sealed` by default unless there's a reason for public/unsealed
- Use `is null` / `is not null` instead of `== null` / `!= null`
- Prefer `record` types for immutable data
- Use primary constructors for DI in services, use cases, handlers
- Prefer `Guid` for identifiers unless told otherwise

## Architecture
- Follow SOLID principles
- Use dependency injection — no service locator patterns
- Prefer controller endpoints over minimal APIs by default; use minimal APIs only for simple endpoints or when explicitly requested
- Use middleware for cross-cutting concerns
- Use `BackgroundService`/`IHostedService` for long-running tasks

## Data & Config
- Entity Framework Core for database access
- Strongly-typed config via `IOptions<T>`
- Environment-specific `appsettings.{Environment}.json`

## API
- Version APIs
- Document with Swagger/OpenAPI
- Validate models (FluentValidation)
- Enforce HTTPS; explicit CORS policy, never permissive-by-default
- Auth/authz on every endpoint

## Testing
- Unit test business logic
- Integration test API endpoints (WebApplicationFactory / AngleSharp / Playwright)
- Test command: `dotnet test`