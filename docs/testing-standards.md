# Testing Standards

This guide covers the conventions and practices for writing tests in this solution. The goals are tests that are easy
to read, easy to maintain, and trustworthy.

Tests use [xUnit](https://xunit.net/), [NSubstitute](https://nsubstitute.github.io/), and
[Shouldly](https://docs.shouldly.org/).

---

## What makes a good unit test

A good unit test is:

- **Fast** - it runs in milliseconds with no I/O or network calls
- **Isolated** - it has no dependencies on external state, files, databases, or other tests
- **Repeatable** - it produces the same result every time it runs
- **Self-checking** - it passes or fails without human interpretation
- **Focused** - it tests one behaviour

---

## Naming

Test method names should read as a plain statement of what the system does under a given condition:

```csharp
public void Organisation_is_closed_at_exact_closing_time_on_a_weekday()
public void Organisation_takes_into_account_daylight_savings_for_open_on_a_weekday()
```

Avoid names that describe the test mechanics (`Test_GetOpenStatus_1`) or that are so generic they provide no signal
when they fail (`Returns_correct_result`).

A good name answers: *what does the system do, and when?*

---

## Structure: Arrange, Act, Assert

Follow the AAA pattern. Use comments to mark each section:

```csharp
[Fact]
public void Laestab_is_separated_into_localAuthorityCode_and_establishmentNumber()
{
    // Arrange
    const string laestab = "3214567";

    // Act
    var laestabValue = new LaestabValue(laestab);

    // Assert
    laestabValue.LocalAuthorityCode.ShouldBe("321");
    laestabValue.EstablishmentNo.ShouldBe("4567");
}
```

When the Act is a single expression, Act and Assert can be combined on one line:

```csharp
[Fact]
public void Organisation_is_closed_at_exact_closing_time_on_a_weekday()
{
    // Arrange
    var exactClosingTime = new DateTime(2026, 2, 11, 15, 30, 0, DateTimeKind.Utc);

    // Act & Assert
    GetStatusAt(exactClosingTime).ShouldBe(OrgStatus.Closed);
}
```

---

## One behaviour per test

Each test should verify a single behaviour. If a test fails, it should be immediately clear what broke.

Avoid multiple Act tasks in one test. If you find yourself asserting several different outcomes, split into separate
tests or use a parameterised `[Theory]`.

---

## Descriptive variable names

Variable names should reflect the actual value and its significance in the scenario, not a generic label:

```csharp
// Good
var exactClosingTime = new DateTime(2026, 2, 11, 15, 30, 0, DateTimeKind.Utc);
var eightThirtyAmBST = new DateTime(2026, 7, 15, 7, 30, 0, DateTimeKind.Utc);

// Avoid
var time = new DateTime(2026, 2, 11, 15, 30, 0, DateTimeKind.Utc);
var dt = new DateTime(2026, 7, 15, 7, 30, 0, DateTimeKind.Utc);
```

A misleading name is worse than a generic one, it actively misdirects the reader.

---

## Reduce repetition with helper methods

When multiple tests share the same setup steps, extract a private helper method rather than repeating the setup in
every test. This keeps each test focused on what makes it distinct:

```csharp
private OrgStatus GetStatusAt(DateTime utc)
{
    _dateTimeProvider.UtcNow.Returns(utc);
    return new StatusCalculator(_dateTimeProvider).GetOpenStatus();
}

[Fact]
public void Organisation_is_open_during_school_hours_on_a_weekday()
{
    // Arrange
    var nineAm = new DateTime(2026, 2, 11, 9, 0, 0, DateTimeKind.Utc);

    // Act & Assert
    GetStatusAt(nineAm).ShouldBe(OrgStatus.Open);
}
```

Prefer helper methods over constructor-based setup. Constructor setup runs for every test regardless of need, can
obscure what each test depends on, and xUnit creates a new class instance per test anyway so the benefit is minimal.

---

## Parameterised tests

When multiple test cases share the same logic and differ only in their inputs, use `[Theory]` with `[InlineData]`
rather than duplicating the test body:

```csharp
[Theory]
[InlineData("", 0)]
[InlineData(",", 0)]
[InlineData("1,2", 3)]
public void Add_returns_expected_sum(string input, int expected)
{
    var calculator = new StringCalculator();

    var actual = calculator.Add(input);

    actual.ShouldBe(expected);
}
```

Use `[Fact]` when a test stands on its own with a clear, distinct name. Use `[Theory]` when you are verifying the
same behaviour across a range of inputs.

---

## Avoid logic in tests

Do not include `if`, `for`, `foreach`, `switch`, or string concatenation in test bodies. Logic in tests creates
the risk of bugs in the tests themselves, which undermines their value.

If you need to cover multiple cases, use `[Theory]` instead.

---

## Avoid infrastructure dependencies

Unit tests must not touch the file system, database, network, or clock. Use interfaces to abstract anything
that would otherwise be a fixed external dependency.

This solution already provides `IDateTimeProvider` as a seam over `DateTime.Now`. Inject it into any class that
needs the current time, and stub it in tests:

```csharp
IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
dateTimeProvider.UtcNow.Returns(new DateTime(2026, 2, 11, 9, 0, 0, DateTimeKind.Utc));
```

---

## Test public behaviour, not private methods

Private methods are implementation details. Test the public method that exercises them. If a private method seems
worth testing directly, consider whether it should be extracted to its own class with a public interface.

---

## Further reading

- [Best practices for writing unit tests (.NET)](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices) - Microsoft Learn
- [You are naming your tests wrong](https://enterprisecraftsmanship.com/posts/you-naming-tests-wrong/) - Enterpise Craftmanship
- [xUnit documentation](https://xunit.net/docs/getting-started/net/cmdline)
- [NSubstitute documentation](https://nsubstitute.github.io/help/getting-started/)
- [Shouldly documentation](https://docs.shouldly.org/)
