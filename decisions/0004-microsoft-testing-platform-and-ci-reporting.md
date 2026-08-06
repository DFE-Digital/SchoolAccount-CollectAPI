---
status: "accepted"
date: "2026-07-23"
decision-makers: Paul Custance
---

# Run Tests on the Microsoft Testing Platform and Publish Results with Community Actions

## Context and Problem Statement

The template initially ran tests through the Visual Studio Test Platform
(VSTest), the long-standing default for `dotnet test`. Microsoft has since
introduced the Microsoft Testing Platform (MTP) as its successor, so the
template has to pick between the two.

Whichever platform runs the tests, its raw output is of little use on its
own. Without further processing, a pull request shows only a pass/fail build
status. Reviewers get no view of which tests ran, what failed, or how
coverage changed.

Two questions follow. Which testing platform should the template run on? And
how do we surface test results and code coverage on every pull request, with
an enforced minimum coverage threshold, without taking on licensing costs?

## Decision Drivers

* Prefer the platform Microsoft is actively investing in; VSTest is in
  maintenance mode and new capability lands in MTP.
* Reviewers should see test results and coverage directly on the pull request,
  not by digging through build logs or downloading artifacts.
* A minimum coverage threshold must fail the build automatically.
* Tests produce one Cobertura file per test project; these must be merged into
  a single figure for the solution.
* No licensing fees, this template is stamped into many projects, so any
  per-repository or per-seat cost multiplies.
* Prefer maintained, widely-used actions over bespoke scripts.

## Considered Options

For the testing platform:

* Microsoft Testing Platform (MTP)
* Visual Studio Test Platform (VSTest)

For publishing results and coverage:

* `EnricoMi/publish-unit-test-result-action@v2` +
  `danielpalme/ReportGenerator-GitHub-Action@5`
* Publish Cobertura files to GitHub's native code coverage
* Hosted coverage/quality services (SonarCloud, Codecov)
* Custom scripts parsing TRX/Cobertura in the workflow

## Decision Outcome

Chosen options: "Microsoft Testing Platform" with
"`EnricoMi/publish-unit-test-result-action@v2` +
`danielpalme/ReportGenerator-GitHub-Action@5`".

MTP is the modern platform and the direction of travel for .NET testing:
tests build as self-contained executables, run faster, and coverage and TRX
reporting are built in (`dotnet test -- --report-trx --coverage
--coverage-output-format cobertura`), removing the separate collector
packages VSTest required. The template switched from VSTest to MTP for this
reason.

The two actions together cover the whole reporting need using free, actively
maintained components that consume the TRX and Cobertura files MTP already
produces. `EnricoMi/publish-unit-test-result-action` reads the TRX files and
publishes a check run with per-test results and failure details.
`danielpalme/ReportGenerator-GitHub-Action` merges the per-project Cobertura
files, fails the build below the minimum line-coverage threshold (currently
50%), and emits a `MarkdownSummaryGithub` report that is written to the job
summary and posted as a sticky PR comment.

### Consequences

* Good, because the template sits on the platform Microsoft is actively
  developing rather than one in maintenance mode.
* Good, because coverage and TRX reporting need no extra collector packages.
* Good, because reviewers see test failures and the coverage summary on the
  pull request itself.
* Good, because the coverage threshold is enforced in the merge step with no
  extra tooling and no licensing fees.
* Bad, because MTP's command-line syntax differs from VSTest
  (arguments pass through after `--`), which can surprise developers used to
  classic `dotnet test` switches.
* Bad, because we depend on two community-maintained actions; a breaking or
  abandoned release would need us to pin, fork, or replace them.
* Bad, because the workflow needs `checks: write` and `pull-requests: write`
  permissions for the actions to publish results and comments.
* Neutral, because coverage history over time is not tracked; each build
  reports a point-in-time figure only.

### Confirmation

Every pull request build demonstrates the decision: tests run through MTP via
the `dotnet test` invocation in
[build.yml](../.github/workflows/build.yml), the "Test Results" check
appears, the coverage summary is posted as a PR comment and job summary, and a
build with line coverage below the threshold fails at the "Merge coverage
reports and enforce threshold" step.

## Pros and Cons of the Options

### Microsoft Testing Platform (MTP)

* Good, because it is where Microsoft's investment and new features go.
* Good, because TRX reporting and code coverage are built in, with no
  `coverlet.collector` or logger packages to manage.
* Good, because test projects are self-contained executables, giving faster
  startup and simpler debugging.
* Bad, because the ecosystem is newer; some tooling and documentation still
  assumes VSTest.

### Visual Studio Test Platform (VSTest)

The classic `dotnet test` runner the template started on.

* Good, because it is mature and universally documented.
* Neutral, because existing workflows and habits already fit it.
* Bad, because it is in maintenance mode and will not receive new investment.
* Bad, because coverage requires additional collector packages and
  configuration that MTP provides out of the box.

### `EnricoMi/publish-unit-test-result-action@v2` + `danielpalme/ReportGenerator-GitHub-Action@5`

* Good, because both are free, popular, and actively maintained.
* Good, because they natively consume the TRX and Cobertura formats MTP emits,
  with no conversion step.
* Good, because ReportGenerator handles merging multiple Cobertura files and
  threshold enforcement in one step.
* Bad, because it is two actions rather than one integrated solution.

### Publish Cobertura files to GitHub's native code coverage

Upload the Cobertura files and let GitHub render coverage on the pull request.

* Good, because it is the most integrated option, with no third-party actions.
* Bad, because GitHub has placed this behind its code quality feature, which
  requires a GitHub Enterprise license and charges $10 per active committer
  per month, failing the no-cost driver for a template stamped into many
  repositories.
* Bad, because it does not publish per-test results from the TRX files, so a
  second tool would still be needed.

### Hosted coverage/quality services (SonarCloud, Codecov)

SonarCloud is the typical enterprise choice in the .NET ecosystem, bundling
coverage with static analysis and quality gates via `dotnet-sonarscanner`;
Codecov is the typical choice in .NET open source and consumes Cobertura
directly.

* Good, because they add coverage history, trends, and diff coverage.
* Good, because SonarCloud adds static analysis and quality gates beyond
  coverage alone.
* Bad, because free tiers are limited: SonarCloud is free only for public
  repositories and charges per lines of code for private ones (SonarQube is
  free only self-hosted, trading fees for operational cost), and Codecov's
  paid tiers are per seat. These costs multiply across every stamped project.
* Bad, because they require an external account, token management, and sending
  source metadata to a third party.

### Custom scripts parsing TRX/Cobertura in the workflow

* Good, because there are no external dependencies at all.
* Bad, because parsing, merging, thresholding, and PR commenting is
  significant bespoke code to maintain in every stamped project; the template
  already moved away from direct scripts in favour of actions.

## More Information

* [Microsoft Testing Platform overview](https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-intro)
* [EnricoMi/publish-unit-test-result-action](https://github.com/EnricoMi/publish-unit-test-result-action)
* [danielpalme/ReportGenerator-GitHub-Action](https://github.com/danielpalme/ReportGenerator-GitHub-Action)
* The sticky PR comment is posted with
  [marocchino/sticky-pull-request-comment](https://github.com/marocchino/sticky-pull-request-comment),
  a supporting detail rather than part of this decision.
* Revisit if GitHub's native coverage reporting becomes available without
  licensing fees.
