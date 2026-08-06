---
status: "accepted"
date: "2026-07-16"
decision-makers: Paul Custance
---

# Structure the solution using clean architecture

## Context and Problem Statement

APIs built from this template will grow domain logic that outlives their
initial CRUD endpoints, and that logic must stay testable and independent of
frameworks and infrastructure. How should the solution be structured so that
business rules do not become entangled with HTTP, persistence, and other
concerns that change for different reasons?

## Decision Drivers

* Domain and application logic must be unit-testable without a database, web
  host, or other infrastructure.
* Infrastructure choices (ORM, database, auth provider) should be replaceable
  without rewriting business rules.
* The structure must be enforceable automatically, not just by convention,
  template consumers will not read a style guide.
* Newcomers should find one obvious place for each kind of code.

## Considered Options

* Clean architecture with one project per layer
* Traditional N-tier layering (UI -> business logic -> data access)
* Vertical slice architecture in a single project
* Single-project minimal API with no enforced layering

## Decision Outcome

Chosen option: "Clean architecture with one project per layer", because it is
the only option that both inverts dependencies away from infrastructure and
can be enforced mechanically at the assembly level.

The solution is split into five projects, with all dependencies pointing
inward:

* `SchoolAccount.SharedKernel` - base building blocks (`Result`, `Error`) with no dependencies.
* `SchoolAccount.Domain` - entities and business rules; depends only on `SchoolAccount.SharedKernel`.
* `SchoolAccount.Application` - command/query handlers for each use case, implementing
  `ICommandHandler`/`IQueryHandler` interfaces. Handlers are decorated with cross-cutting
  concerns such as validation and logging. Depends on `SchoolAccount.Domain` and
  `SchoolAccount.SharedKernel`.
* `SchoolAccount.Infrastructure` - EF Core/PostgreSQL persistence, authentication, time -
  implementations of abstractions the inner layers define; depends on
  `SchoolAccount.Application`.
* `SchoolAccount.Web.Api` - minimal API endpoints and composition root; references
  `SchoolAccount.Infrastructure` only to wire dependency injection.

### Consequences

* Good, because handlers and domain types are testable in isolation; the
  architecture tests run with no infrastructure at all.
* Good, because layer violations fail the build rather than accumulating
  silently.
* Bad, because simple features carry ceremony: one endpoint typically means an
  endpoint class, a query/command, a handler, and a response type across two
  projects.
* Bad, because five projects is a heavier starting point than a single-project
  API for genuinely small services.

### Confirmation

`tests/SchoolAccount.ArchitectureTests` encodes the dependency rules with NetArchTest (e.g.
`SchoolAccount.Domain` must not depend on `SchoolAccount.Application`,
`SchoolAccount.Application` must not depend on `SchoolAccount.Infrastructure`). These tests run in the
`Build` workflow on every pull request, so a violating change cannot merge cleanly.

## More Information

* [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) -
  Robert C. Martin's original description of the dependency rule.
* [NetArchTest](https://github.com/BenMorris/NetArchTest) - the library used
  by the architecture tests.
* Related: [ADR-0001](0001-record-architecture-decisions.md) established this
  decision log.
