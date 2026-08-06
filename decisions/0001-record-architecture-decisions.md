---
status: "accepted"
date: "2026-07-16"
decision-makers: Paul Custance
---

# Use Markdown Architectural Decision Records

## Context and Problem Statement

Every project stamped from this template inherits its architectural choices,
but the reasoning behind them is only discoverable through commits and PRs. 
How do we record what was decided, why, and what was rejected, so
the record lives with the code and travels into every new project?

## Decision Drivers

* Decisions must be discoverable by new joiners and by teams stamping projects
  from this template, without access to historical conversations.
* Recording a decision must be lightweight enough that it actually happens.
* Records must be reviewable through the same pull-request process as the code
  they describe.

## Considered Options

* Markdown Architectural Decision Records (MADR) in the repository
* Nygard-style ADRs in the repository
* Decisions documented in an external wiki
* No formal decision records

## Decision Outcome

Chosen option: "Markdown Architectural Decision Records (MADR) in the
repository", because the records live and travel with the code, and MADR's
explicit Considered Options / Pros and Cons sections force the rejected
alternatives to be captured, which is the information most often lost.

Records are kept in `decisions/`, named `NNNN-short-kebab-case-title.md` and
numbered sequentially, using the template in
[0000-adr-template.md](0000-adr-template.md). An ADR is written when a
decision is expensive to reverse, constrains future work, or resolves a debate
the team is likely to revisit. Accepted ADRs are immutable: a change of
direction is a new ADR that supersedes the old one, updating the old record's
status to "superseded by ADR-NNNN".

### Consequences

* Good, because the reasoning behind non-obvious choices is documented where
  future maintainers and template consumers will actually find it.
* Good, because projects stamped from this template inherit the convention and
  the existing decisions and can supersede them where their context differs.
* Bad, because proposing a significant change gains a small writing overhead.

### Confirmation

ADRs are reviewed through the normal pull-request process alongside the change
they describe; reviewers check that significant changes reference a new or
existing ADR.

## More Information

* [MADR project](https://adr.github.io/madr/) - the template's home and full
  documentation.
* [ADR GitHub organisation](https://adr.github.io/) - background on
  architectural decision records generally.
