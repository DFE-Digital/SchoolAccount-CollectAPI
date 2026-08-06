# Introduction

SchoolAccount-ApiTemplate is a template for building ASP.NET Core Web APIs for the DfE School Account service. It
provides a minimal clean architecture solution, with CQRS abstractions, structured logging, error handling, and
architecture tests already wired up, so new backend services can start from a consistent, proven baseline rather than
from scratch.

## Documentation

- [Clean Architecture](docs/clean-architecture.md) - layers, dependency rules, and code organisation
- [Coding Standards](docs/coding-standards.md) - formatting, code analysis, naming, and style conventions
- [Testing Standards](docs/testing-standards.md) - conventions and practices for writing tests
- [Integration Testing](docs/integration-testing.md) - guidance on integration testing of the API endpoints
- [Open API Documentation](docs/open-api-documentation.md) - guidance on adding Open API documentation to endpoints

Architecture decisions are recorded as ADRs in the [decisions](decisions) folder:

- [Use Markdown Architectural Decision Records](decisions/0001-record-architecture-decisions.md) - why and how we record decisions
- [Structure the solution using clean architecture](decisions/0002-use-clean-architecture.md) - layers, dependency rules, and code organisation
- [Strip the imported template to a minimal core](decisions/0003-strip-imported-template-to-minimal-core.md) - what was removed from the original template and why
- [Run tests on the Microsoft Testing Platform](decisions/0004-microsoft-testing-platform-and-ci-reporting.md) - testing platform and how results and coverage are reported in CI
- [Format code with CSharpier](decisions/0005-format-code-with-csharpier.md) - why formatting is automated and enforced in the build
- [Enforce code quality with Roslyn analysers](decisions/0006-enforce-code-quality-with-roslyn-analysers.md) - why SonarAnalyzer.CSharp and strict analysis are enforced in the build
- [Validate requests with Data Annotations](decisions/0007-validate-requests-with-data-annotations.md) - why request shape is validated with Data Annotations rather than a FluentValidation decorator

New decisions should follow the [ADR template](decisions/0000-adr-template.md).

# Getting Started

Follow these steps to start the API locally.

**Note:** Windows users can use the `git bash` command prompt to run the project's `.sh` bash scripts.

1. Install prerequisites:
    - [.NET 10 SDK](https://dotnet.microsoft.com/download)
    - [Docker Desktop](https://www.docker.com/products/docker-desktop/)
    - Rider, Visual Studio, or Visual Studio Code

2. Run the setup script from the repository root to restore the dotnet tools and enable the git hooks:

   ```bash
   ./init.sh
   ```

3. Run the API using one of the following:

   | Method         | Command                                          | Outcome                                                              |
   |----------------|--------------------------------------------------|----------------------------------------------------------------------|
   | Docker Compose | `docker compose up --build`                      | Starts the API and its dependencies (Seq) in containers              |
   | .NET CLI       | `dotnet run --project src/SchoolAccount.Web.Api` | Runs the API directly using the `http` launch profile, no containers |

   In Rider or Visual Studio you can use the equivalent `docker-compose` or `http` run configurations from the toolbar.

4. Once running, the API is available at `http://localhost:5100`:
    - Interactive API reference (Scalar) at `http://localhost:5100/scalar/v1`
    - Health checks at `http://localhost:5100/health`
    - Logs (if started with compose) at `http://localhost:8081`

   > The Scalar API reference is only mapped in the `Development` environment.

5. Debugging guidance:
    - Set breakpoints in your C# files under `src/` and start either run configuration with debugging enabled.
    - `.http` files alongside the endpoints in `src/SchoolAccount.Web.Api/Endpoints` can be used to exercise the API from your IDE.

# Build and Test

Use the .NET CLI to build or test the solution.

- To build locally:

  ```bash
  dotnet build
  ```

- To run all tests:

  ```bash
  dotnet test
  ```

Architecture tests under `tests/SchoolAccount.ArchitectureTests` enforce the clean architecture dependency rules between layers.

### Formatting

Code is formatted with [CSharpier](https://csharpier.com/), installed as a local dotnet tool and enforced by the
"Check formatting" step in the [build workflow](.github/workflows/build.yml). To format the solution locally:

```bash
dotnet csharpier format .
```

A pre-commit hook, managed by [Husky.NET](https://alirezanet.github.io/Husky.Net/) and configured in
[.husky/task-runner.json](.husky/task-runner.json), formats staged C# files automatically before each commit;
[init.sh](init.sh) installs it and restores the tools on a fresh clone. To format on save, install the
[Rider plugin](https://plugins.jetbrains.com/plugin/18243-csharpier) or the
[VS Code extension](https://marketplace.visualstudio.com/items?itemName=csharpier.csharpier-vscode); the
[editors documentation](https://csharpier.com/docs/Editors) covers setup for these and other IDEs. See
[Format code with CSharpier](decisions/0005-format-code-with-csharpier.md) for the reasoning.

### Code Analysis

The build runs with the full set of .NET/Roslyn analyzers (`AnalysisLevel=latest`, `AnalysisMode=All`) plus
[SonarAnalyzer.CSharp](https://www.sonarsource.com/products/sonarlint/), configured in
[Directory.Build.props](Directory.Build.props). `TreatWarningsAsErrors` means any violation fails `dotnet build`,
locally and in the [build workflow](.github/workflows/build.yml), rather than being left as a warning. Rules that
don't fit this codebase are suppressed by ID in [.editorconfig](.editorconfig). See
[Coding Standards](docs/coding-standards.md) and
[Enforce code quality with Roslyn analysers](decisions/0006-enforce-code-quality-with-roslyn-analysers.md) for the
conventions and the reasoning.

### Code Coverage

The [build workflow](.github/workflows/build.yml) collects code coverage on every run, posts a summary to the pull
request, and fails the build if line coverage drops below the minimum threshold. The threshold is defined by the
`MIN_LINE_COVERAGE` variable at the top of [build.yml](.github/workflows/build.yml). Which files are included is
controlled by [coverage.config](coverage.config).

To generate the same report locally, run [coverage.sh](coverage.sh) from the repository root:

```bash
./coverage.sh
```

The script runs all tests with coverage enabled, merges the per-project results with ReportGenerator, and writes an
HTML report to `TestResults/CoverageReport/index.html`. Pass `--open` to open the report in your browser when it
finishes:

```bash
./coverage.sh --open
```

## Architecture

The solution follows a clean architecture pattern with vertical slice features:

| Project                        | Purpose                                                      |
|--------------------------------|--------------------------------------------------------------|
| `SchoolAccount.Web.Api`        | ASP.NET Core Web API - endpoints, middleware, error handling |
| `SchoolAccount.Application`    | CQRS handlers and feature logic, organised by feature folder |
| `SchoolAccount.Domain`         | Domain entities and business rules                           |
| `SchoolAccount.Infrastructure` | External concerns - time, data access, integrations          |
| `SchoolAccount.SharedKernel`   | Shared primitives - `Result<T>`, `Error`, `ValidationError`  |

Each endpoint implements `IEndpoint` and is discovered and mapped automatically at startup. See
[Structure the solution using clean architecture](decisions/0002-use-clean-architecture.md) for the dependency rules.

## Logging

Structured logs are written via Serilog to [Seq](https://datalust.co/seq). When running via Docker Compose, the Seq UI
is available at http://localhost:8081.

## Contributing

1. Branch from `main` using the convention `task/<short-description>` or `feature/<short-description>`.
2. Open a [pull request](https://github.com/DFE-Digital/SchoolAccount-ApiTemplate/pulls) against `main`.
3. The [build workflow](.github/workflows/build.yml) must pass before merging.
