---
status: "accepted"
date: "2026-07-24"
decision-makers: Paul Custance
---

# Format Code with CSharpier

## Context and Problem Statement

Code formatting in the template is currently whatever each developer's IDE
produces. The team works across Rider, Visual Studio, and Visual Studio Code,
and each applies its own defaults on top of the `.editorconfig`. The result
is inconsistent formatting between files, avoidable diff noise, and pull
request comments about layout rather than behaviour.

Projects stamped from this template inherit the same problem. How do we keep
formatting consistent across every editor and every stamped project, and
enforce it automatically rather than through review comments?

## Decision Drivers

* Formatting must be identical regardless of which IDE or editor wrote the
  code.
* The build must enforce it; formatting feedback in PR review is too late and
  wastes reviewer attention.
* Minimal configuration to maintain. Style debates cost more than any single
  style choice is worth, especially multiplied across stamped projects.
* Fast enough to run on every build and on save without being noticed.
* Pinned tooling, so every developer and CI run uses the same version.

## Considered Options

* CSharpier
* `dotnet format`
* EditorConfig with IDE formatting only
* StyleCop.Analyzers

## Decision Outcome

Chosen option: "CSharpier", because it is the only option that is fully
deterministic: it reprints the whole file from the syntax tree, so the same
code always produces the same output regardless of what it looked like
before. It is opinionated with almost nothing to configure, which removes
style debate, and it is fast enough to check the whole solution in well under
a second.

CSharpier is installed as a pinned local dotnet tool in
[.config/dotnet-tools.json](../.config/dotnet-tools.json). Developers format
with `dotnet csharpier format .`, and most IDEs can run CSharpier on save via
its official plugins. The build runs `dotnet csharpier check .` and fails on
any file that does not match the expected formatting. The `.idea` directory
is excluded via [.csharpierignore](../.csharpierignore).

### Consequences

* Good, because formatting is identical everywhere and enforced by the build,
  so pull requests contain no formatting churn or review comments about
  layout.
* Good, because the tool version is pinned in the manifest and restored with
  `dotnet tool restore`, giving identical results locally and in CI.
* Bad, because CSharpier's style is fixed; the few opinions it holds (such as
  line width and brace placement) cannot be tuned to personal taste.
* Bad, because the initial repository-wide reformat touches most files,
  adding noise to `git blame` around the adoption commit.
* Neutral, because CSharpier only covers formatting; code-quality rules
  remain the job of the existing analysers and `.editorconfig`.

### Confirmation

The "Check formatting" step in
[build.yml](../.github/workflows/build.yml) runs `dotnet csharpier check .`
on every push and pull request and fails the build if any file is not
formatted. Locally, a [Husky.NET](https://alirezanet.github.io/Husky.Net/)
pre-commit hook, configured in
[.husky/task-runner.json](../.husky/task-runner.json) and installed by
[init.sh](../init.sh), runs `csharpier format` on staged C# files and
re-stages them before the commit completes.

## Pros and Cons of the Options

### CSharpier

An opinionated formatter for C#, in the mould of Prettier.

* Good, because output is deterministic; it reprints from the syntax tree
  rather than nudging existing layout.
* Good, because there is almost nothing to configure, so nothing to debate or
  maintain.
* Good, because it is fast (the whole solution checks in under a second) and
  has plugins for Rider, Visual Studio, and VS Code.
* Bad, because its opinions are fixed and occasionally differ from what a
  team member would choose.

### `dotnet format`

Microsoft's formatter, driven by `.editorconfig` rules.

* Good, because it is first-party and needs no extra tooling.
* Good, because style is configurable through `.editorconfig`.
* Bad, because it only fixes rule violations rather than reprinting, so files
  can satisfy the rules while still being formatted inconsistently.
* Bad, because it is markedly slower, which discourages running it on every
  build.
* Bad, because configurability reopens the style debates a formatter should
  close.

### EditorConfig with IDE formatting only

The status quo: `.editorconfig` plus whatever each IDE does.

* Good, because it requires no new tooling.
* Bad, because each IDE interprets and supplements the rules differently, so
  formatting drifts between developers.
* Bad, because nothing enforces it; inconsistencies surface as PR noise.

### StyleCop.Analyzers

Style analysers that report violations as build warnings or errors.

* Good, because enforcement happens in the compiler with no separate step.
* Bad, because it is a linter rather than a formatter: it reports problems
  but developers still fix layout by hand.
* Bad, because its large rule set needs curating and maintaining, which is
  the configuration burden this decision tries to avoid.

## More Information

* [CSharpier documentation](https://csharpier.com/)
* Adopting CSharpier surfaced a line-ending mismatch: `.editorconfig`
  declared `end_of_line = crlf` while git normalised files to LF, which would
  have failed the format check on Linux CI. The `.editorconfig` now declares
  `lf` so the editor, git, and CSharpier agree.
* Revisit only if CSharpier becomes unmaintained; its style choices are
  explicitly not grounds for revisiting.
