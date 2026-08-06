# Clean Architecture

Clean Architecture is a software design philosophy that separates the elements of a design into ring levels. The
foundational rule is simple:

> **Dependencies can only point inward.** Outer layers depend on inner layers, never the reverse.

This concept was introduced by **[Robert C. Martin](https://en.wikipedia.org/wiki/Robert_C._Martin)** (Uncle Bob) in
his influential 2012 article [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).
He also published the book [Clean Architecture: A Craftsman's Guide to Software Structure and Design](https://www.oreilly.com/library/view/clean-architecture-a/9780134494166/),
which expands on these principles in depth.

This constraint is what gives Clean Architecture its power. It keeps your business logic free of framework concerns,
makes the application independently testable, and means you can swap out infrastructure (databases, APIs, message
queues) without touching your domain.

This template is based primarily on the practical .NET application of Clean Architecture by **[Milan Jovanović](https://www.milanjovanovic.tech/)** -
Software Architect and Microsoft MVP.

---

## Uncle Bob's Original Vision

Robert C. Martin's original diagram visualises the architecture as concentric rings, with the innermost ring containing
the most stable business rules and each outer ring adding layers of detail and infrastructure concerns.

The four circles represent:

1. **Entities (innermost)** - Enterprise-wide business rules that remain true regardless of which application uses them
2. **Use Cases** - Application-specific business logic that orchestrates data flow with Entities
3. **Interface Adapters** - Controllers, Gateways, Presenters that convert data between the application and external systems (web, database, etc.)
4. **Frameworks & Drivers (outermost)** - Web frameworks, databases, and other external libraries kept at arm's length

Uncle Bob's foundational principle: **"source code dependencies can only point inward."** Nothing in an inner circle
can know anything about something in an outer circle.

Read [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) - the original article that started it all.

---

## The Four Layers

This solution is split into four projects, each representing one layer:

### 1. Domain

The innermost ring. It has **no dependencies on any other project** in the solution.

Contains:
- **Entities** - objects with identity and lifecycle
- **Value Objects** - immutable descriptors with no identity
- **Domain Events** - things that happened in the domain
- **Repository interfaces** - abstractions over data access (implementations live in Infrastructure)
- **Domain exceptions** - business rule violations

This is where your business rules live. If you can describe a rule without mentioning a database or an HTTP request,
it belongs here.

### 2. Application

Sits directly above Domain. Acts as an **orchestrator** - it coordinates domain objects to fulfil use cases,
but contains no business rules itself.

Contains:
- **Commands and Queries** (CQRS) - one class per use case
- **Command/Query handlers** - the logic that executes each use case
- **Abstractions** - interfaces for things the Application layer needs but does not implement (email, storage, messaging)
- **Pipeline behaviours** - cross-cutting concerns like logging and validation

The Application layer depends on Domain. It never depends on Infrastructure or Presentation.

### 3. Infrastructure

Implements the abstractions defined by the Application layer. Everything that talks to the outside world lives here.

Contains:
- **Database access and configuration**
- **External service clients** (email, blob storage, message brokers)
- **Background jobs**
- **Authentication integrations**

Infrastructure depends on both Domain and Application but is never referenced by them.

### 4. Presentation

The entry point to the system - the Web API project. In this solution that is `SchoolAccount.Web.Api`.

Contains:
- **Endpoints** - thin wrappers implementing `IEndpoint` that dispatch Commands and Queries
- **Middleware** - request pipeline concerns
- **Error handling** - exception handling and mapping `Result` failures to HTTP problem details
- **Authentication configuration**

The Presentation layer depends on Application (to send commands/queries) but should not reference Infrastructure directly.

---

## The Dependency Rule in Practice

```
Domain ← Application ← Infrastructure
              ↑
         Presentation
```

The arrows show the direction of **allowed** dependencies. No arrow ever points toward Infrastructure from the inner layers.

These rules are enforced by the tests in `tests/SchoolAccount.ArchitectureTests`.

---

## How This Solution Applies Clean Architecture

| Layer          | Project                        |
|----------------|--------------------------------|
| Domain         | `SchoolAccount.Domain`         |
| Application    | `SchoolAccount.Application`    |
| Infrastructure | `SchoolAccount.Infrastructure` |
| Presentation   | `SchoolAccount.Web.Api`        |
| Shared kernel  | `SchoolAccount.SharedKernel`   |

`SchoolAccount.SharedKernel` contains primitives used across all layers (`Result<T>`, `Error`, `ValidationError`,
`IDateTimeProvider`) and sits outside the ring model - it has no dependencies itself and can be referenced by any
layer.

The Application layer organises features using **package by feature** under `src/SchoolAccount.Application/`,
with each use case in its own folder.

---

## Further Reading

### Original Work (Uncle Bob)

- [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) (2012) - Robert C. Martin's foundational article
- [Clean Architecture: A Craftsman's Guide to Software Structure and Design](https://www.oreilly.com/library/view/clean-architecture-a/9780134494166/) - Robert C. Martin's comprehensive book

### .NET Implementation (Milan Jovanović)

- [Clean Architecture in .NET - Complete Guide](https://www.milanjovanovic.tech/blog/clean-architecture-dotnet) - Milan Jovanović
- [How To Approach Clean Architecture Folder Structure](https://www.milanjovanovic.tech/blog/clean-architecture-folder-structure) - Milan Jovanović
- [Clean Architecture: The Missing Chapter](https://www.milanjovanovic.tech/blog/clean-architecture-the-missing-chapter) - Milan Jovanović
- [Pragmatic Clean Architecture course](https://www.milanjovanovic.tech/pragmatic-clean-architecture) - Milan Jovanović
