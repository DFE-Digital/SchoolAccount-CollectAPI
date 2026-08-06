---
status: "accepted"
date: "2026-07-27"
decision-makers: Paul Custance
---

# Enforce Code Quality with Roslyn Analysers

## Context and Problem Statement

Bugs, code smells, and maintainability issues (unreachable code, needless complexity, redundant
conditions, and similar) are currently only caught by whoever happens to notice them in review,
or not at all. CSharpier already makes formatting consistent and automatic, but formatting says
nothing about whether the code is correct or well-structured.

Projects stamped from this template inherit the same gap. How do we catch these issues
automatically, at build time, across every stamped project, without relying on reviewer
attention?

## Decision Drivers

* Issues should surface at build time, on every push, not during manual review or in production.
* The same baseline must apply to every project stamped from this template with no extra setup.
* A curated rule set. A large, unfiltered rule set produces enough noise that warnings stop being
  trusted.
* Pinned tooling, so every developer and CI run analyses the same way.
* Should complement CSharpier, not duplicate it. Formatting and code quality are different
  concerns.

## Considered Options

* .NET/Roslyn built-in analyzers with SonarAnalyzer.CSharp, `TreatWarningsAsErrors`
* SonarQube/SonarCloud as a separate CI stage
* StyleCop.Analyzers
* Code review only, no additional analysers

## Decision Outcome

Chosen option: "built-in analyzers with SonarAnalyzer.CSharp, `TreatWarningsAsErrors`", because it
runs inside the existing `dotnet build`, needs no separate service or CI stage, and its findings
are curated over time in `.editorconfig` rather than accepted wholesale.

[Directory.Build.props](../Directory.Build.props) sets `AnalysisLevel` to `latest` and
`AnalysisMode` to `All`, turning on the full set of built-in .NET/Roslyn analyzers, and adds
[SonarAnalyzer.CSharp](https://www.sonarsource.com/products/sonarlint/) as an analyzer-only
package reference for additional bug and code-smell rules. `TreatWarningsAsErrors` and
`CodeAnalysisTreatWarningsAsErrors` are both `true`, so any violation fails the build rather than
sitting as an ignorable warning, and `EnforceCodeStyleInBuild` brings `.editorconfig` style rules
into the same gate. Individual rules that do not suit this codebase are suppressed by ID in
[.editorconfig](../.editorconfig), each with a comment naming the rule.

### Consequences

* Good, because bugs and code smells are caught on every build rather than depending on a
  reviewer noticing them.
* Good, because the baseline is defined once in `Directory.Build.props` and applies to every
  project stamped from the template with no per-project configuration.
* Good, because the analyser version is pinned in
  [Directory.Packages.props](../Directory.Packages.props), giving identical results locally and
  in CI.
* Bad, because `TreatWarningsAsErrors` turns any newly introduced rule (from an SDK or
  SonarAnalyzer upgrade) into a build break across the whole solution, which can block unrelated
  pull requests until triaged.
* Bad, because some rules do not fit this codebase's patterns and need an explicit suppression in
  `.editorconfig`, which is an ongoing maintenance cost as the rule sets evolve.
* Neutral, because analysis only covers code quality; formatting remains CSharpier's job (see
  [Format code with CSharpier](0005-format-code-with-csharpier.md)).

### Confirmation

The "Build" step in [build.yml](../.github/workflows/build.yml) runs `dotnet build` with
`TreatWarningsAsErrors` active, so any analyser violation fails CI on every push and pull request.
The same failure happens locally on `dotnet build`, so there is no separate step to remember to
run.

## Pros and Cons of the Options

### .NET/Roslyn built-in analyzers with SonarAnalyzer.CSharp, `TreatWarningsAsErrors`

* Good, because it runs as part of the existing build, with no separate service, license, or CI
  stage to maintain.
* Good, because `AnalysisMode=All` plus SonarAnalyzer covers a broad range of bug patterns and
  code smells beyond what the built-in analyzers alone catch.
* Good, because rules can be suppressed individually in `.editorconfig`, so the set can be tuned
  to the codebase over time.
* Bad, because a strict `AnalysisMode` combined with `TreatWarningsAsErrors` can require
  attention on every SDK or package upgrade if it introduces new rules.

### SonarQube/SonarCloud as a separate CI stage

* Good, because it gives a persistent dashboard of code quality trends across time and projects.
* Bad, because it requires hosting or a paid subscription, and a separate CI stage to maintain.
* Bad, because findings surface after the build rather than failing it, so violations can still
  merge unless a separate quality gate is wired up.

### StyleCop.Analyzers

* Good, because it runs in the build like the chosen option.
* Bad, because it is a style linter rather than a bug/code-smell detector, so it overlaps with
  CSharpier and `.editorconfig` rather than adding new coverage.

### Code review only, no additional analysers

* Good, because it requires no tooling or configuration.
* Bad, because it depends entirely on reviewer attention, which is inconsistent and does not
  scale across projects stamped from this template.

## More Information

* [SonarSource rules for C#](https://rules.sonarsource.com/csharp/) documents every SonarAnalyzer
  rule.
* Suppressed rules are listed with a short comment in the `.NET Code Analyzers rules`,
  `IDE Code Analyzers rules`, and `SonarAnalyzer.CSharp rules` sections of
  [.editorconfig](../.editorconfig).
* Revisit a specific rule by suppressing it in `.editorconfig` when it produces more noise than
  value for this codebase; that is a routine tuning decision, not grounds for revisiting this ADR
  as a whole.
