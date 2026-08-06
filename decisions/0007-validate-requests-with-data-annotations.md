---
status: "accepted"
date: "2026-07-28"
decision-makers: Paul Custance
---

# Validate Requests with Data Annotations

## Context and Problem Statement

Incoming requests need their shape validated (required fields, format, length) before reaching an
Application layer query or command handler. The solution already has a decorator pattern for
wrapping handlers, used by `LoggingDecorator`, so a similar decorator wrapping
`IQueryHandler`/`ICommandHandler` with FluentValidation is one natural place to put this. Where
should request validation live, and with what tool?

This also needs to separate two different kinds of check that are easy to conflate. Boundary
validation asks whether a request is well-formed, such as whether a string has the right shape, a
required field is present, or a number falls in range, and it only ever needs the request itself.
Business-rule validation asks whether an operation is allowed to happen at all, such as whether an
email already exists, an organisation is already closed, or a referenced record exists. It needs
data from outside the request, typically the database or other domain state.

Queries rarely need the second kind. A query doesn't change anything, so there is no invariant to
protect; if the data it asks for doesn't exist, that's an ordinary query outcome (a `NotFound`
`Result`), not a rule violation. Commands are where business-rule validation matters, because a
command changes state and has invariants to protect before it's allowed to. This solution has no
concrete commands yet, only the `ICommand`/`ICommandHandler` abstractions, so the decision below is
really about where boundary validation lives today, and where business-rule validation would go
once commands exist.

## Decision Drivers

* A validation failure should produce a standard problem-details response with no handler code.
* Requests should be validated consistently without every handler re-implementing shape checks.
* Minimal ceremony for simple, common cases: required, format, length, range.
* Prefer a framework feature that already does the job over adding a library and a pipeline step.
* Boundary validation (is this request well-formed) and business-rule validation (is this
  operation allowed) are different concerns and should not be forced into the same mechanism.
* Business-rule validation that needs data outside the request itself (uniqueness, lookups,
  cross-aggregate rules) still needs somewhere to live, even if it isn't the default path.

## Considered Options

* Data Annotations on the request record, validated automatically by ASP.NET Core
* FluentValidation validators wrapped around query/command handlers via a decorator
* FluentValidation validators invoked directly inside each endpoint delegate
* No structured validation, manual checks inside each handler

## Decision Outcome

Chosen option: "Data Annotations on the request record, validated automatically by ASP.NET Core",
because request-shape constraints are declared directly on the field they constrain and ASP.NET
Core validates them automatically before the endpoint delegate runs, with no decorator or
validator class needed for what is usually a handful of attributes.

`services.AddValidation()` in
[SchoolAccount.Web.Api/DependencyInjection.cs](../src/SchoolAccount.Web.Api/DependencyInjection.cs) wires
up ASP.NET Core's built-in minimal API validation. `GetByLaestabRequest.Laestab` in
[GetByLaestabRequest.cs](../src/SchoolAccount.Web.Api/Endpoints/Organisations/GetByLaestab/GetByLaestabRequest.cs)
carries a `[RegularExpression]` attribute as the example. A malformed value never reaches
`GetOrganisationByLaestabQueryHandler`; the endpoint's `.ProducesValidationProblem()` documents the
response the client gets instead.

This covers boundary validation for both queries and commands, checking whether the request is
well-formed enough to act on at all, and it deliberately does not try to cover business-rule
validation. Queries, like the one example in this codebase, need nothing more than this, because a
query has no state-changing invariant to protect. "The LAESTAB isn't a real organisation" is a
`NotFound` result from the handler, not a validation failure at the boundary. When a command is
added and needs a rule that depends on other data, that validation belongs in the Application
layer, next to the handler it protects, the same way `LoggingDecorator` sits next to the handlers
it wraps. It should be added for that specific command rather than adopted as a blanket mechanism
up front.

### Consequences

* Good, because the constraint lives next to the field it constrains, on the request the client
  actually sends, so there is one place to read to know what a request must look like.
* Good, because no extra pipeline step (decorator, behavior, mediator step) is needed; validation
  happens before the endpoint delegate runs.
* Good, because the response is ASP.NET Core's standard `ValidationProblem`, matching what
  `.ProducesValidationProblem()` already documents on every endpoint.
* Good, because it keeps boundary and business-rule validation visibly separate. Request shape is
  an attribute on the request, while an operation's business rules are code in its handler, so a
  reader never has to guess which one a given check belongs to.
* Bad, because Data Annotations can only validate the shape of a single request in isolation;
  anything needing other data (a database lookup, a cross-field business rule) has no attribute to
  express it and must be checked inside the handler instead.
* Neutral, because `FluentValidation.DependencyInjectionExtensions` remains referenced in
  [SchoolAccount.Application.csproj](../src/SchoolAccount.Application/SchoolAccount.Application.csproj)
  for that case. Nothing currently
  uses it; if a query or command needs it, wrap that specific handler the way `LoggingDecorator`
  does, rather than making FluentValidation the default for every handler.

### Confirmation

[GetByLaestabTests.cs](../tests/SchoolAccount.Web.Api.IntegrationTests/EndPoints/Organisations/GetByLaestabTests.cs)
exercises the endpoint with a malformed LAESTAB value and asserts the validation problem response,
confirming the request never reaches the query handler.

## Pros and Cons of the Options

### Data Annotations on the request record, validated automatically by ASP.NET Core

* Good, because it is a framework feature with nothing extra to install or wire beyond
  `AddValidation()`.
* Good, because the constraint and the field are declared in the same place.
* Bad, because it only covers single-request shape checks, not rules that need other data.

### FluentValidation validators wrapped around query/command handlers via a decorator

* Good, because it centralises validation in the Application layer, alongside the handler it
  guards, and can express rules that need other data or services.
* Bad, because every query and command needs its own validator class and decorator registration
  even for the simplest shape checks, which is disproportionate ceremony for the common case.
* Bad, because request shape becomes an Application-layer concern instead of living on the request
  the client actually sends.

### FluentValidation validators invoked directly inside each endpoint delegate

* Good, because it needs no decorator or pipeline wiring.
* Bad, because it must be repeated by hand in every endpoint delegate, with no framework
  enforcement that a given request type is actually validated.

### No structured validation, manual checks inside each handler

* Good, because it requires no tooling.
* Bad, because checks are inconsistent between handlers and easy to forget, and every handler must
  build its own error response instead of reusing a standard one.

## More Information

* Revisit if a query or command needs validation that depends on data beyond the request itself;
  at that point, add a FluentValidation validator for that specific handler rather than changing
  the default for every request.
* Queries are expected to stay boundary-only. If a query ever seems to need a business-rule check,
  that's usually a sign the check belongs in a command instead, since queries have no state to
  protect.
