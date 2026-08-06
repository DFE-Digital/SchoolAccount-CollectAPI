# Coding Standards

This guide covers the conventions that keep code in this solution consistent and the automated
checks that enforce it. Where a rule is encoded in tooling rather than convention, this guide
points at the config file rather than duplicating it, so there is one source of truth to keep up
to date.

---

## Formatting

Formatting is not a matter of taste here: [CSharpier](https://csharpier.com/) reprints every file
from its syntax tree, and the build fails if a file does not match. Run
`dotnet csharpier format .` before committing, or format on save with the
[Rider plugin](https://plugins.jetbrains.com/plugin/18243-csharpier) or the
[VS Code extension](https://marketplace.visualstudio.com/items?itemName=csharpier.csharpier-vscode).
The [CSharpier editors documentation](https://csharpier.com/docs/Editors) covers setup for these
and other IDEs. A Husky pre-commit hook formats staged files automatically regardless of editor.

See [Format code with CSharpier](../decisions/0005-format-code-with-csharpier.md) for why this is
enforced rather than left to each editor's defaults.

---

## Code analysis and quality gates

Every project in the solution builds with the .NET/Roslyn analyzers set to their strictest level
(`AnalysisLevel=latest`, `AnalysisMode=All`) plus
[SonarAnalyzer.CSharp](https://www.sonarsource.com/products/sonarlint/) for additional bug and
code-smell detection, all defined centrally in
[Directory.Build.props](../Directory.Build.props). `TreatWarningsAsErrors` means any violation
fails the build, not just the IDE's squiggly underline.

If a specific rule does not fit this codebase, suppress it by ID in
[.editorconfig](../.editorconfig) with a comment naming the rule, rather than scattering
`#pragma warning disable` through the code. This keeps every suppression visible and reviewable
in one place.

See
[Enforce code quality with Roslyn analysers](../decisions/0006-enforce-code-quality-with-roslyn-analysers.md)
for the reasoning and trade-offs.

---

## Language and style

- Favour explicit typing. Only use `var` when the type is obvious from the right-hand side.
- Make types `internal sealed` by default. Only widen to `public` or remove `sealed` when there
  is a reason to.
- Use `is null` / `is not null` rather than `== null` / `!= null`.
- Prefer `record` types for immutable data.
- Use primary constructors for dependency injection in services, use cases, and handlers.
- Prefer `Guid` for identifiers unless told otherwise.
- Nullable reference types are enabled solution-wide; do not disable them per-file.
- Namespaces are file-scoped and using directives are placed outside the namespace.

These and the finer-grained rules (brace style, expression-bodied members, `var` usage, and so
on) are enforced through [.editorconfig](../.editorconfig), which is the source of truth if this
list and the file ever disagree.

---

## Naming

- Types are `PascalCase`.
- Interfaces are `PascalCase` prefixed with `I`.
- Non-field members (properties, events, methods) are `PascalCase`.
- Private and internal fields are `_camelCase`, prefixed with an underscore.

See the `#### Naming styles ####` section of [.editorconfig](../.editorconfig) for the full rule
definitions.

---

## Architecture and testing

Coding standards cover how individual files are written. For how projects relate to each other,
see [Clean Architecture](clean-architecture.md). For how tests are written, see
[Testing Standards](testing-standards.md) and [Integration Testing](integration-testing.md).
